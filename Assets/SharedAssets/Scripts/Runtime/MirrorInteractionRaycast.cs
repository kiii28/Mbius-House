using UnityEngine;

public class MirrorInteractionRaycast : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private PlayerInventory sharedInventory; // Menghubungkan ke inventori utama atau inventori terpisah

    public int mirrorKeyCount = 0; // Kunci yang dibawa karakter cermin

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // 1. Ambil Kunci
                InteractableItem item = hit.collider.GetComponent<InteractableItem>();
                if (item != null && item.itemType == ItemType.Key)
                {
                    mirrorKeyCount++;
                    if (sharedInventory != null) sharedInventory.CollectKey();
                    Destroy(hit.collider.gameObject);
                    Debug.Log($"Karakter cermin mengambil kunci! Total: {mirrorKeyCount}");
                    return;
                }

                // 2. Buka Pintu Terkunci
                LockedDoor door = hit.collider.GetComponent<LockedDoor>();
                if (door != null)
                {
                    door.TryOpenDoor(this);
                    return;
                }
            }
        }
    }
}