using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GameObject interactPromptUI;

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        bool hitSomething = Physics.Raycast(ray, out hit, interactDistance, interactableLayer);

        if (interactPromptUI != null)
        {
            interactPromptUI.SetActive(hitSomething);
        }

        if (hitSomething && Input.GetKeyDown(KeyCode.E))
        {
            // 1. Cek pemicu Quest (seperti tong sampah / dumpster)
            QuestTriggerItem questItem = hit.collider.GetComponent<QuestTriggerItem>();
            if (questItem != null)
            {
                questItem.TriggerQuest();
                return;
            }

            // 2. Cek item inventori (kunci / fragmen)
            InteractableItem item = hit.collider.GetComponent<InteractableItem>();
            if (item != null)
            {
                item.Interact(playerInventory);
                return;
            }

            // 3. Cek kasur tidur
            BedInteraction bed = hit.collider.GetComponent<BedInteraction>();
            if (bed != null)
            {
                bed.SleepAndTeleport(transform.root.gameObject);
                return;
            }
        }
    }
}