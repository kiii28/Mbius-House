using System.Collections;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public static DayNightController Instance;

    [Header("Sun / Moon Light")]
    [SerializeField] private Light directionalLight;

    [Header("Day Settings")]
    [SerializeField] private Vector3 dayRotation = new Vector3(50f, -30f, 0f);
    [SerializeField] private Color dayLightColor = Color.white;
    [SerializeField] private float dayIntensity = 1.2f;
    [SerializeField] private Color dayAmbientColor = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField] private Color dayFogColor = new Color(0.7f, 0.8f, 0.9f);

    [Header("Night Settings")]
    [SerializeField] private Vector3 nightRotation = new Vector3(170f, -30f, 0f);
    [SerializeField] private Color nightLightColor = new Color(0.1f, 0.15f, 0.25f);
    [SerializeField] private float nightIntensity = 0.05f;
    [SerializeField] private Color nightAmbientColor = new Color(0.02f, 0.02f, 0.05f); // Langit malam pekat
    [SerializeField] private Color nightFogColor = new Color(0.01f, 0.01f, 0.03f);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetDayInstant();
    }

    public void SetDayInstant()
    {
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(dayRotation);
            directionalLight.color = dayLightColor;
            directionalLight.intensity = dayIntensity;
        }

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = dayAmbientColor;
        RenderSettings.fog = true;
        RenderSettings.fogColor = dayFogColor;
        RenderSettings.fogDensity = 0.005f;
    }

    public void TransitionToNight(float duration = 2.5f)
    {
        StartCoroutine(NightTransitionRoutine(duration));
    }

    private IEnumerator NightTransitionRoutine(float duration)
    {
        float t = 0;
        Quaternion startRot = directionalLight.transform.rotation;
        Quaternion endRot = Quaternion.Euler(nightRotation);
        Color startColor = directionalLight.color;
        float startIntensity = directionalLight.intensity;

        // Ambil kamera utama
        Camera mainCam = Camera.main;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            if (directionalLight != null)
            {
                directionalLight.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
                directionalLight.color = Color.Lerp(startColor, nightLightColor, t);
                directionalLight.intensity = Mathf.Lerp(startIntensity, 0f, t);
            }

            // Matikan kabut siang, pekatkan kabut hitam
            RenderSettings.fogColor = Color.Lerp(Color.white, Color.black, t);
            RenderSettings.fogDensity = Mathf.Lerp(0.002f, 0.05f, t);

            yield return null;
        }

        // PAKSA LANGIT MENJADI HITAM:
        if (mainCam != null)
        {
            mainCam.clearFlags = CameraClearFlags.SolidColor;
            mainCam.backgroundColor = new Color(0.02f, 0.02f, 0.05f); // Warna langit malam gelap
        }
    }
}