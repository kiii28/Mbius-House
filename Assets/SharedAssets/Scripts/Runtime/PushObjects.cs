using UnityEngine;

public class PushObjects : MonoBehaviour
{
    [SerializeField] private float pushPower = 2.0f;
    [SerializeField] private string pushableTag = "Pushable"; // Tag khusus agar tidak semua benda terdorong

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Cek apakah objek punya Rigidbody dan bukan kinetik
        if (body == null || body.isKinematic) return;

        // Pastikan hanya objek dengan Tag tertentu yang bisa didorong
        if (!hit.gameObject.CompareTag(pushableTag)) return;

        // Jangan dorong benda yang ada di bawah kaki (lantai)
        if (hit.moveDirection.y < -0.3f) return;

        // Hitung arah dorongan hanya secara horizontal (X dan Z)
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Berikan gaya dorong pada Rigidbody
        body.linearVelocity = pushDir * pushPower;
    }
}