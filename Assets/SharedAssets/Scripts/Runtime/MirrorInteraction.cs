using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{
    [Header("UI Interaksi")]
    [SerializeField] private GameObject interactUI; // Masukkan objek InteractPrompt di sini

    [Header("Skrip Kontrol")]
    [SerializeField] private MonoBehaviour mainPlayerMovement;
    [SerializeField] private MonoBehaviour mainPlayerLook;
    [SerializeField] private MonoBehaviour mirrorPlayerMovement;

    public bool isControllingMirror = false;
    private bool playerInRange = false;

    void Start()
    {
        if (interactUI != null) interactUI.SetActive(false);
    }

    void Update()
    {
        // Mendukung input lama & baru
        bool pressE = false;
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null)
            pressE = UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame;
#endif
        if (Input.GetKeyDown(KeyCode.E)) pressE = true;

        if (playerInRange && pressE)
        {
            ToggleMirrorControl(!isControllingMirror);
        }
    }

    public void ToggleMirrorControl(bool controlMirror)
    {
        isControllingMirror = controlMirror;

        mainPlayerMovement.enabled = !controlMirror;
        mainPlayerLook.enabled = !controlMirror;
        mirrorPlayerMovement.enabled = controlMirror;

        // Sembunyikan teks interaksi saat sedang mengendalikan cermin
        if (interactUI != null) interactUI.SetActive(!controlMirror && playerInRange);

        Cursor.lockState = controlMirror ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = controlMirror;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactUI != null && !isControllingMirror) interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactUI != null) interactUI.SetActive(false);
            if (isControllingMirror) ToggleMirrorControl(false);
        }
    }
}