using UnityEngine;

public class CorridorGenerator : MonoBehaviour
{
    public static CorridorGenerator Instance;

    [Header("Prefabs")]
    [SerializeField] private GameObject corridorPrefab;
    [SerializeField] private GameObject finalRoomPrefab;

    [Header("Urutan Jalur Benar (6 Loop)")]
    [SerializeField]
    private TurnDirection[] pathSequence = new TurnDirection[6]
    {
        TurnDirection.Kanan, // Loop 1
        TurnDirection.Kiri,  // Loop 2
        TurnDirection.Kanan, // Loop 3
        TurnDirection.Kanan, // Loop 4
        TurnDirection.Kiri,  // Loop 5
        TurnDirection.Kanan  // Loop 6 -> menuju Final Room
    };

    public int currentLoopIndex = 0;

    [Header("Lorong Aktif Saat Ini")]
    [SerializeField] private GameObject currentCorridor;
    private GameObject previousCorridor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Konfigurasi lorong pertama yang ada di scene sejak awal
        if (currentCorridor != null)
        {
            CorridorSegment seg = currentCorridor.GetComponent<CorridorSegment>();
            if (seg != null && pathSequence.Length > 0)
            {
                seg.ConfigureCorrectPath(pathSequence[0]);
            }
        }
    }

    public void SpawnNextSegment(Transform connectionPoint)
    {
        currentLoopIndex++;

        bool isFinalRoom = currentLoopIndex >= pathSequence.Length;
        GameObject prefabToSpawn = isFinalRoom ? finalRoomPrefab : corridorPrefab;

        // 1. Spawn prefab baru
        GameObject newSegment = Instantiate(prefabToSpawn, connectionPoint.position, connectionPoint.rotation);

        // 2. Cari EntrancePoint
        Transform newEntrance = newSegment.transform.Find("EntrancePoint");
        if (newEntrance != null)
        {
            // Hitung selisih rotasi dasar
            Quaternion deltaRot = connectionPoint.rotation * Quaternion.Inverse(newEntrance.rotation);

            // KUNCI PERBAIKAN: Tambahkan putaran 180 derajat pada sumbu Y agar tidak terbalik
            deltaRot *= Quaternion.Euler(0f, 180f, 0f);

            // Terapkan rotasi yang sudah dikoreksi
            newSegment.transform.rotation = deltaRot * newSegment.transform.rotation;

            // Geser posisi agar EntrancePoint menempel pas di bibir connectionPoint
            Vector3 deltaPos = connectionPoint.position - newEntrance.position;
            newSegment.transform.position += deltaPos;
        }

        // 3. Konfigurasi belokan lorong baru jika belum final
        if (!isFinalRoom)
        {
            CorridorSegment seg = newSegment.GetComponent<CorridorSegment>();
            if (seg != null)
            {
                seg.ConfigureCorrectPath(pathSequence[currentLoopIndex]);
            }
        }

        // 4. Hapus lorong lama di belakang
        if (previousCorridor != null)
        {
            Destroy(previousCorridor);
        }

        previousCorridor = currentCorridor;
        currentCorridor = newSegment;
    }

    public void ResetProgress()
    {
        currentLoopIndex = 0;

        if (currentCorridor != null)
        {
            CorridorSegment seg = currentCorridor.GetComponent<CorridorSegment>();
            if (seg != null && pathSequence.Length > 0)
            {
                seg.ConfigureCorrectPath(pathSequence[0]);
            }
        }

        if (PlayerHUDManager.Instance != null)
        {
            PlayerHUDManager.Instance.UpdateMemoryProgress(0, pathSequence.Length);
        }
    }
}