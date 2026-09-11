using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName = "Gameplay";

    private bool isTransitioning = false;

    void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
    }

    IEnumerator Start()
    {
        // Pastikan video tidak langsung play sendiri sebelum siap
        videoPlayer.playOnAwake = false;
        videoPlayer.Prepare();

        // Tunggu sampai video selesai dipersiapkan di memori
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();

        // Beri jeda 1 detik sebelum mulai mendengarkan event akhir video
        // (mencegah false trigger di frame awal)
        yield return new WaitForSeconds(1f);
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (isTransitioning) return;

        bool skip = false;

#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame ||
                UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
                skip = true;
        }
#endif
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            skip = true;

        if (skip)
        {
            PindahKeGameplay();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        PindahKeGameplay();
    }

    private void PindahKeGameplay()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        if (videoPlayer != null) videoPlayer.Stop();

        if (LoadingScreen.Instance != null)
        {
            LoadingScreen.Instance.LoadScene(nextSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}