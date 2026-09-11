using UnityEngine;
using TMPro;

public class PrologueQuestManager : MonoBehaviour
{
    public static PrologueQuestManager Instance;

    [Header("UI Objective (Opsional jika pakai PlayerHUDManager)")]
    [SerializeField] private TextMeshProUGUI questObjectiveText;

    [Header("Quest Status")]
    public bool isTaskCompleted = false;
    public bool isNightTime = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateQuestText("Beli perbekalan / Ambil barang di toko kota");
    }

    public void CompleteCityTask()
    {
        if (isTaskCompleted) return;

        isTaskCompleted = true;
        isNightTime = true;

        if (DayNightController.Instance != null)
        {
            DayNightController.Instance.TransitionToNight(2.5f);
        }

        UpdateQuestText("Hari sudah larut malam. Kembali ke rumah dan tidur di kasur.");
    }

    public void UpdateQuestText(string text)
    {
        if (questObjectiveText != null)
        {
            questObjectiveText.text = text;
        }

        if (PlayerHUDManager.Instance != null)
        {
            PlayerHUDManager.Instance.SetQuestText(text);
        }
    }
}