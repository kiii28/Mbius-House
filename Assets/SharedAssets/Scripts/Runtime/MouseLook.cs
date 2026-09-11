using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        // Kunci kursor di tengah layar dan sembunyikan
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Gerakan kamera ke atas/bawah (Pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Batasi agar kepala tidak terbalik

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Putar badan karakter ke kiri/kanan (Yaw)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}