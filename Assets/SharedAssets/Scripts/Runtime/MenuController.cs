using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    [Header("UI Panels")]
    [SerializeField] private GameObject mainPanel;      // Panel menu utama
    [SerializeField] private GameObject settingsPanel;  // Panel menu settings

    [Header("Settings UI Elements")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Dropdown qualityDropdown;  // Gunakan TMP_Dropdown jika pakai TextMeshPro
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private AudioMixer audioMixer;      // Opsional: jika pakai AudioMixer

    private void Start()
    {
        // Pastikan mouse bebas dan terlihat saat berada di menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Pastikan hanya panel utama yang aktif saat start
        if (mainPanel != null) mainPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Load nilai pengaturan tersimpan (PlayerPrefs)
        LoadSettingsPreferences();

    }

    // --- 1. NEW GAME ---
    public void NewGame()
    {
        PlayerPrefs.SetInt("IsLoadedGame", 0);
        PlayerPrefs.Save();

        // Pindah ke Scene Cutscene menggunakan Loader Realtime
        if (LoadingScreen.Instance != null)
        {
            LoadingScreen.Instance.LoadScene("CutsceneScene"); // atau nama scene tujuanmu
        }
        else
        {
            SceneManager.LoadScene("CutsceneScene");
        }
    }

    // --- 2. LOAD GAME ---
    public void LoadGame()
    {
        // Cek apakah ada file save / tanda simpanan data
        if (PlayerPrefs.HasKey("HasSaveData"))
        {
            PlayerPrefs.SetInt("IsLoadedGame", 1);
            PlayerPrefs.Save();

            // Pindah ke scene gameplay (logika pemulihan posisi dijalankan di scene Gameplay)
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            Debug.LogWarning("Belum ada data permainan tersimpan!");
        }
    }

    // --- 3. NAVIGASI PANEL SETTINGS ---
    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
        PlayerPrefs.Save(); // Simpan perubahan setting
    }

    // --- 4. FUNGSI PENGATURAN (SETTINGS) ---
    public void SetVolume(float volume)
    {
        // Jika pakai AudioListener langsung:
        AudioListener.volume = volume;

        // Simpan ke PlayerPrefs
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualitySetting", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("IsFullscreen", isFullscreen ? 1 : 0);
    }

    // --- 5. LOAD PENGATURAN AWAL ---
    private void LoadSettingsPreferences()
    {
        // Volume
        if (volumeSlider != null)
        {
            float savedVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
            volumeSlider.value = savedVol;
            AudioListener.volume = savedVol;
        }

        // Kualitas Grafis
        if (qualityDropdown != null)
        {
            int savedQuality = PlayerPrefs.GetInt("QualitySetting", QualitySettings.GetQualityLevel());
            qualityDropdown.value = savedQuality;
            QualitySettings.SetQualityLevel(savedQuality);
        }

        // Fullscreen
        if (fullscreenToggle != null)
        {
            bool isFull = PlayerPrefs.GetInt("IsFullscreen", Screen.fullScreen ? 1 : 0) == 1;
            fullscreenToggle.isOn = isFull;
            Screen.fullScreen = isFull;
        }
    }

    // --- 6. QUIT GAME ---
    public void QuitGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();
    }
}