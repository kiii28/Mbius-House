using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryEffectManager : MonoBehaviour
{
    public static MemoryEffectManager Instance;

    [Header("UI Flashback")]
    [SerializeField] private CanvasGroup flashbackCanvasGroup; // Panel transparan di Canvas
    [SerializeField] private TextMeshProUGUI memoryText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip whisperSFX;

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerMemory(string textNarrative, float duration = 4f)
    {
        StartCoroutine(PlayFlashbackRoutine(textNarrative, duration));
    }

    private IEnumerator PlayFlashbackRoutine(string narrative, float duration)
    {
        if (audioSource != null && whisperSFX != null)
            audioSource.PlayOneShot(whisperSFX);

        if (memoryText != null)
            memoryText.text = narrative;

        // Fade in ke layar putih / kabur
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            if (flashbackCanvasGroup != null) flashbackCanvasGroup.alpha = t;
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        // Fade out kembali ke gameplay normal
        while (t > 0f)
        {
            t -= Time.deltaTime * 1.5f;
            if (flashbackCanvasGroup != null) flashbackCanvasGroup.alpha = t;
            yield return null;
        }

        if (memoryText != null) memoryText.text = "";
    }
}