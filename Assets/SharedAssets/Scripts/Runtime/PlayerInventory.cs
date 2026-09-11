using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Flashlight")]
    [SerializeField] private GameObject flashlightObject;
    public bool hasFlashlight = true; // Senter sudah ada dari awal
    private bool isFlashlightOn = false;

    [Header("Keys")]
    public int keyCount = 0;

    [Header("Memories")]
    public int memoriesFound = 0;
    public const int TotalMemories = 6;

    private void Start()
    {
        // Kondisi awal senter mati
        if (flashlightObject != null)
            flashlightObject.SetActive(false);

        // Update tampilan HUD awal
        if (PlayerHUDManager.Instance != null)
        {
            PlayerHUDManager.Instance.UpdateFlashlightHUD(isFlashlightOn);
            PlayerHUDManager.Instance.UpdateMemoryProgress(memoriesFound, TotalMemories);
        }
    }

    private void Update()
    {
        // Toggle senter dengan tombol F
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    // Dipanggil saat mengambil kunci
    public void CollectKey()
    {
        keyCount++;
        Debug.Log($"Kunci diambil! Total kunci: {keyCount}");
    }

    // Dipanggil jika senter dipungut manual
    public void CollectFlashlight()
    {
        hasFlashlight = true;
        isFlashlightOn = true;
        if (flashlightObject != null)
            flashlightObject.SetActive(true);

        if (PlayerHUDManager.Instance != null)
            PlayerHUDManager.Instance.UpdateFlashlightHUD(true);
    }

    // Dipanggil saat menemukan potongan ingatan
    public void CollectMemoryShard()
    {
        memoriesFound++;
        if (PlayerHUDManager.Instance != null)
        {
            PlayerHUDManager.Instance.UpdateMemoryProgress(memoriesFound, TotalMemories);
        }
    }

    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        if (flashlightObject != null)
            flashlightObject.SetActive(isFlashlightOn);

        if (PlayerHUDManager.Instance != null)
            PlayerHUDManager.Instance.UpdateFlashlightHUD(isFlashlightOn);
    }
}