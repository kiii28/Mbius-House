using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsPanelUI;

    [Header("Nama Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public static bool isGamePaused = false;

    void Start()
    {
        Resume(); // Pastikan game mulai dalam keadaan tidak ter-pause
    }

    void Update()
    {
        bool pressEsc = false;
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null)
            pressEsc = UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame;
#endif
        if (Input.GetKeyDown(KeyCode.Escape)) pressEsc = true;

        if (pressEsc)
        {
            if (isGamePaused)
            {
                // Jika sedang buka panel setting di dalam pause menu, tutup setting dulu
                if (settingsPanelUI != null && settingsPanelUI.activeSelf)
                {
                    CloseSettings();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    // 1. RESUME
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (settingsPanelUI != null) settingsPanelUI.SetActive(false);

        Time.timeScale = 1f; // Waktu game berjalan normal
        isGamePaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 2. PAUSE
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        if (settingsPanelUI != null) settingsPanelUI.SetActive(false);

        Time.timeScale = 0f; // Bekukan seluruh pergerakan di game
        isGamePaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 3. SAVE
    public void SaveGame()
    {
        // Contoh penyimpanan sederhana menggunakan PlayerPrefs
        PlayerPrefs.SetInt("HasSaveData", 1);

        // Simpan posisi karakter utama
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);
        }

        PlayerPrefs.Save();
        Debug.Log("Game berhasil disimpan!");
    }

    // 4. SETTINGS
    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsPanelUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanelUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    // 5. MAIN MENU
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Wajib dinormalkan sebelum pindah scene
        isGamePaused = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}