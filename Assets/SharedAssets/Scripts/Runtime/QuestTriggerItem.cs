using UnityEngine;

public class QuestTriggerItem : MonoBehaviour
{
    [SerializeField] private bool destroyOnInteract = false;

    public void TriggerQuest()
    {
        if (PrologueQuestManager.Instance != null)
        {
            PrologueQuestManager.Instance.CompleteCityTask();
        }

        if (destroyOnInteract)
        {
            Destroy(gameObject);
        }
    }
}