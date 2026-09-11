using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;

    [Header("UI References")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncRoutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Pengaman: jika nama scene salah atau belum didaftarkan di build settings
        if (operation == null)
        {
            Debug.LogError($"Gagal memuat scene '{sceneName}'. Pastikan sudah ditambahkan ke File -> Build Settings!");
            if (loadingPanel != null) loadingPanel.SetActive(false);
            yield break;
        }

        operation.allowSceneActivation = false;

        float progress = 0f;

        while (!operation.isDone)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            progress = Mathf.MoveTowards(progress, targetProgress, 1.5f * Time.unscaledDeltaTime);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = $"{(progress * 100f):F0}%";

            if (progress >= 0.99f && operation.progress >= 0.9f)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}