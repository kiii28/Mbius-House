using System.Collections;
using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    [Header("Teleport Target")]
    [SerializeField] private Transform forestSpawnPoint;

    [Header("Fade UI")]
    [SerializeField] private CanvasGroup blackScreenFade; // Panel hitam fullscreen

    [Header("Audio")]
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioClip forestNightBGM;

    
    public void SleepAndTeleport(GameObject player)
    {

        if (!PrologueQuestManager.Instance.isNightTime)
        {
            Debug.Log("Belum malam, selesaikan quest terlebih dahulu!");
            return;
        }

        StartCoroutine(SleepRoutine(player));
    }

    private IEnumerator SleepRoutine(GameObject player)
    {
        // 1. Matikan kontrol pemain sementara agar tidak bergerak
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // 2. Fade to black (tidur)
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f;
            if (blackScreenFade != null) blackScreenFade.alpha = t;
            yield return null;
        }

        yield return new WaitForSeconds(2f); // Suasana hening tidur

        // 3. Pindahkan posisi player ke hutan
        if (forestSpawnPoint != null)
        {
            player.transform.position = forestSpawnPoint.position;
            player.transform.rotation = forestSpawnPoint.rotation;
        }

        // 4. Ganti BGM ke suasana hutan horor
        if (ambientAudioSource != null && forestNightBGM != null)
        {
            ambientAudioSource.clip = forestNightBGM;
            ambientAudioSource.Play();
        }

        PrologueQuestManager.Instance.UpdateQuestText("Jelajahi hutan dan cari jalan menuju vila.");

        // 5. Fade in dari layar hitam
        while (t > 0f)
        {
            t -= Time.deltaTime * 1.2f;
            if (blackScreenFade != null) blackScreenFade.alpha = t;
            yield return null;
        }

        // 6. Aktifkan kembali kontrol pemain
        if (cc != null) cc.enabled = true;
    }
}