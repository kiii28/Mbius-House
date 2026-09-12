using UnityEngine;

public enum TurnDirection { Kiri, Kanan }

public class CorridorSegment : MonoBehaviour
{
    [Header("Titik Sambung (Snap Points)")]
    public Transform entrancePoint;
    public Transform connectionPointKiri;
    public Transform connectionPointKanan;

    [Header("Trigger Belokan")]
    [SerializeField] private ProceduralBranchTrigger triggerKiri;
    [SerializeField] private ProceduralBranchTrigger triggerKanan;

    public void ConfigureCorrectPath(TurnDirection correctDirection)
    {
        if (correctDirection == TurnDirection.Kiri)
        {
            triggerKiri.SetupTrigger(true, connectionPointKiri, entrancePoint);
            triggerKanan.SetupTrigger(false, null, entrancePoint);
        }
        else
        {
            triggerKiri.SetupTrigger(false, null, entrancePoint);
            triggerKanan.SetupTrigger(true, connectionPointKanan, entrancePoint);
        }
    }
}