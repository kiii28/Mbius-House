using System.Collections;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private bool requiresKey = true;
    [SerializeField] private Vector3 openRotationOffset = new Vector3(0f, 90f, 0f);
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip openSFX;
    [SerializeField] private AudioClip lockedSFX;

    [Header("Mirror Integration")]
    [SerializeField] private MirrorInteraction mirrorInteraction; // Referensi ke script pengatur cermin

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + openRotationOffset);
    }

    public void TryOpenDoor(MirrorInteractionRaycast mirrorUser)
    {
        if (isOpen) return;

        if (requiresKey)
        {
            if (mirrorUser != null && mirrorUser.mirrorKeyCount > 0)
            {
                mirrorUser.mirrorKeyCount--;
                StartCoroutine(OpenDoorRoutine(mirrorUser));
            }
            else
            {
                if (doorAudioSource != null && lockedSFX != null)
                    doorAudioSource.PlayOneShot(lockedSFX);
                Debug.Log("Pintu terkunci! Butuh kunci.");
            }
        }
        else
        {
            StartCoroutine(OpenDoorRoutine(mirrorUser));
        }
    }

    private IEnumerator OpenDoorRoutine(MirrorInteractionRaycast mirrorUser)
    {
        isOpen = true;
        if (doorAudioSource != null && openSFX != null)
            doorAudioSource.PlayOneShot(openSFX);

        // 1. Matikan kontrol cermin & kembalikan ke player utama
        if (mirrorInteraction != null)
        {
            mirrorInteraction.ToggleMirrorControl(false);
        }

        // 2. Hilangkan karakter cermin dari dunia game
        if (mirrorUser != null)
        {
            // Ambil root object dari karakter cermin (mirrorCharacther)
            GameObject mirrorChar = mirrorUser.transform.root.gameObject;
            mirrorChar.SetActive(false); // Bisa juga Destroy(mirrorChar) jika tidak dipakai lagi
        }

        // 3. Putar animasi pintu terbuka
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Slerp(closedRotation, openRotation, t);
            yield return null;
        }
    }
}