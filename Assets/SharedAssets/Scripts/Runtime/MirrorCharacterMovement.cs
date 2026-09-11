using UnityEngine;

public class MirrorCharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform mirrorCameraTransform;

    [Header("Physics")]
    [SerializeField] private float gravity = 15f;
    private float verticalVelocity = 0f;

    void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        enabled = false; // Aktif saat pemain interaksi ke cermin
    }

    void Update()
    {
        if (controller == null || !controller.enabled) return;

        // 1. Tangani gravitasi dengan batas aman (mencegah akumulasi kecepatan gila)
        if (controller.isGrounded)
        {
            verticalVelocity = -1f; // Tekan sedikit ke lantai agar isGrounded stabil
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            verticalVelocity = Mathf.Clamp(verticalVelocity, -20f, 10f); // KUNCI: Batasi kecepatan jatuh maksimal -20
        }

        // 2. Baca input WASD
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        Vector3 moveDir = Vector3.zero;

        if (inputDir.sqrMagnitude > 0.01f)
        {
            if (mirrorCameraTransform != null)
            {
                Vector3 camForward = mirrorCameraTransform.forward;
                Vector3 camRight = mirrorCameraTransform.right;
                camForward.y = 0;
                camRight.y = 0;
                camForward.Normalize();
                camRight.Normalize();

                moveDir = (camRight * inputDir.x + camForward * inputDir.z).normalized;
            }
            else
            {
                moveDir = inputDir;
            }

            // Rotasi menghadap arah jalan
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // 3. Gabungkan gerak horizontal dan gravitasi
        Vector3 motion = (moveDir * moveSpeed) + new Vector3(0, verticalVelocity, 0);
        controller.Move(motion * Time.deltaTime);
    }
}