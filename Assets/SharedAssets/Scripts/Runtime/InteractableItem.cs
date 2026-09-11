using UnityEngine;

public enum ItemType { Key, Flashlight, MemoryShard }

public class InteractableItem : MonoBehaviour
{
    public ItemType itemType;

    [Header("Khusus Memory Shard")]
    [TextArea(2, 4)]
    [SerializeField] private string memoryDialogue = "Sebuah fragmen memori berkelebat...";
    [SerializeField] private int memoryID = 1; // 1 sampai 6

    [Header("Audio SFX (Opsional)")]
    [SerializeField] private AudioClip pickupSFX;

    public void Interact(PlayerInventory player)
    {
        // Putar audio pengambilan jika ada
        if (pickupSFX != null)
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position);

        switch (itemType)
        {
            case ItemType.Key:
                player.CollectKey();
                Destroy(gameObject);
                break;

            case ItemType.Flashlight:
                player.CollectFlashlight();
                Destroy(gameObject);
                break;

            case ItemType.MemoryShard:
                player.memoriesFound++;
                if (MemoryEffectManager.Instance != null)
                {
                    MemoryEffectManager.Instance.TriggerMemory(memoryDialogue);
                }
                Debug.Log($"Memori #{memoryID} ditemukan! ({player.memoriesFound}/{PlayerInventory.TotalMemories})");
                Destroy(gameObject);
                break;
        }
    }
}