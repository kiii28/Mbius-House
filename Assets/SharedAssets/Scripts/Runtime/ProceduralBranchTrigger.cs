using UnityEngine;

public class ProceduralBranchTrigger : MonoBehaviour
{
    private bool isCorrectTurn = false;
    private Transform connectionPoint;
    private Transform loopResetPoint;
    private bool triggered = false;

    public void SetupTrigger(bool isCorrect, Transform snapPoint, Transform resetPoint)
    {
        isCorrectTurn = isCorrect;
        connectionPoint = snapPoint;
        loopResetPoint = resetPoint;
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        if (isCorrectTurn)
        {
            triggered = true;
            if (CorridorGenerator.Instance != null)
            {
                CorridorGenerator.Instance.SpawnNextSegment(connectionPoint);
            }
        }
        else
        {
            // Reset hitungan kembali ke awal
            if (CorridorGenerator.Instance != null)
            {
                CorridorGenerator.Instance.ResetProgress();
            }

            // Kembalikan pemain ke pangkal lorong
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            if (loopResetPoint != null)
            {
                other.transform.position = loopResetPoint.position;
                other.transform.rotation = loopResetPoint.rotation;
            }

            if (cc != null) cc.enabled = true;
        }
    }
}