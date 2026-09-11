using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    public static PlayerHUDManager Instance;

    [Header("Quest UI (Pojok Kiri Atas)")]
    [SerializeField] private TextMeshProUGUI questDetailText;
    [SerializeField] private TextMeshProUGUI memoryCounterText;

    [Header("Flashlight UI (Pojok Kanan Bawah)")]
    [SerializeField] private Image flashlightIcon;
    [SerializeField] private TextMeshProUGUI flashlightStatusText;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(0.4f, 0.4f, 0.4f, 0.6f);

    private void Awake()
    {
        Instance = this;
    }

    public void SetQuestText(string questObjective)
    {
        if (questDetailText != null)
            questDetailText.text = questObjective;
    }

    public void UpdateMemoryProgress(int current, int max)
    {
        if (memoryCounterText != null)
        {
            if (current <= 0)
            {
                // Sembunyikan jika belum mulai misi vila/ingatan
                memoryCounterText.gameObject.SetActive(false);
            }
            else
            {
                memoryCounterText.gameObject.SetActive(true);
                memoryCounterText.text = $"Potongan Ingatan: {current}/{max}";
            }
        }
    }

    public void UpdateFlashlightHUD(bool isOn)
    {
        if (flashlightIcon != null)
            flashlightIcon.color = isOn ? activeColor : inactiveColor;

        if (flashlightStatusText != null)
        {
            flashlightStatusText.text = isOn ? "[F] HIDUP" : "[F] MATI";
            flashlightStatusText.color = isOn ? activeColor : inactiveColor;
        }
    }
}