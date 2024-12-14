using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WaterSpreadManager : MonoBehaviour
{
    // �������� ��� ����������� �������
    public static WaterSpreadManager Instance { get; private set; }

    [Header("������������ ��������� ����")]
    [SerializeField] private Tilemap waterTilemap; // ������� ����� ��� ����
    [SerializeField] private LayerMask obstacleLayer; // ��� ��� ������������� ��������
    [SerializeField] private TileBase waterTile; // ����, �� ����������� ����

    [Header("��������� ���������")]
    [SerializeField] private float liquidDissipationRate = 0.1f; // �������� ������������� ����
    [SerializeField] private float minLiquidThreshold = 0.1f; // ̳������� ������� ���� ��� ���������
    [SerializeField] private bool allowDiagonalSpread = true; // ��������� ���������� ���������

    // ������ �������� ���� � ���������
    private Dictionary<Vector3Int, WaterCell> activeWaterCells = new Dictionary<Vector3Int, WaterCell>();
    private Queue<(WaterCell cell, int remainingSteps)> spreadEvents = new Queue<(WaterCell, int)>(); // ����� ���� ��������� ���� � ���������� �����

    private void Awake()
    {
        // ��������� ���������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ����������� ���������� ������� �����, ���� �� ������
        if (waterTilemap == null)
            waterTilemap = FindObjectOfType<Tilemap>();

        // ������������, ���� ���� ���� �� �����������
        if (waterTile == null)
        {
            Debug.LogWarning("���� ���� �� �������. ���� �����, ��������� ���� � ��������� ��� ����� ���.");
        }
    }

    private void Start()
    {
        // ������ �������� ��������� ��������� ����
        StartCoroutine(WaterSpreadUpdateCycle());
    }

    private System.Collections.IEnumerator WaterSpreadUpdateCycle()
    {
        // ������������ ���� ��� ������� ��������� ����
        while (true)
        {
            ProcessWaterSpread(Time.deltaTime); // �������� deltaTime ��� ����������� ���������
            yield return null; // ���������� �� ���������� �����
        }
    }

    /// <summary>
    /// ������ ��������� ���� � ������ ����� ����.
    /// </summary>
    /// <param name="worldPosition">������ ������� ��� ������� ���������.</param>
    /// <param name="amount">��������� ������� ����.</param>
    /// <param name="spreadPressure">���������� ���� ���������.</param>
    /// <param name="maxSteps">����������� ������� ����� ���������.</param>
    public void SpreadWater(Vector3 worldPosition, float amount, float spreadPressure, int maxSteps)
    {
        if (waterTilemap == null || waterTile == null)
        {
            Debug.LogError("�������� ��������� ���� ������������ �����������!");
            return;
        }

        // ������������ ������ ������� � ������� �� ����
        Vector3Int gridPosition = waterTilemap.WorldToCell(worldPosition);

        // ����������� ������ ��������, ���� �� ����������
        if (!activeWaterCells.ContainsKey(gridPosition))
        {
            WaterCell newWaterCell = new WaterCell(gridPosition, amount)
            {
                SpreadPressure = spreadPressure
            };

            activeWaterCells[gridPosition] = newWaterCell;
            spreadEvents.Enqueue((newWaterCell, maxSteps));

            // ������������ ����� ���� �� ������� ����
            waterTilemap.SetTile(gridPosition, waterTile);
        }
    }

    /// <summary>
    /// �������� ����� ��������� ����, ������������� ����� �� ��������� �����.
    /// </summary>
    /// <param name="deltaTime">���, �� ����� � ���������� ���������.</param>
    private void ProcessWaterSpread(float deltaTime)
    {
        Dictionary<Vector3Int, WaterCell> newWaterCells = new();

        int processedEvents = 0;
        while (spreadEvents.Count > 0 && processedEvents < 50) // ��������� ������� ���� �� ����
        {
            var (currentCell, remainingSteps) = spreadEvents.Dequeue();

            // ��������� ������� ����� �� ����� � �����
            currentCell.LiquidAmount -= liquidDissipationRate * deltaTime;
            currentCell.SpreadPressure *= Mathf.Pow(0.8f, deltaTime);

            // ��������� ��������, ���� ������� ���� ����� ������
            if (currentCell.LiquidAmount <= minLiquidThreshold || remainingSteps <= 0)
            {
                activeWaterCells.Remove(currentCell.GridPosition);
                waterTilemap.SetTile(currentCell.GridPosition, null);
                continue;
            }

            // �������� ������ �������� ��� ���������
            foreach (Vector3Int direction in GetSpreadDirections())
            {
                Vector3Int neighborPos = currentCell.GridPosition + direction;

                // ����������� �������, �� ����� ���������
                Vector3 worldPos = waterTilemap.GetCellCenterWorld(neighborPos);
                if (Physics2D.OverlapPoint(worldPos, obstacleLayer) != null)
                    continue;

                // ���������� ������� ���� ��� ��������� (���������� ��� ����������� ��������)
                float spreadAmount = currentCell.SpreadPressure * (direction.magnitude > 1 ? 0.5f : 1f);

                // ��������� ������ �������� ���� ��� ��������� ���������
                if (!activeWaterCells.TryGetValue(neighborPos, out WaterCell neighborCell))
                {
                    neighborCell = new WaterCell(neighborPos, spreadAmount);
                    newWaterCells[neighborPos] = neighborCell;
                    spreadEvents.Enqueue((neighborCell, remainingSteps - 1));
                    waterTilemap.SetTile(neighborPos, waterTile);
                }
                else
                {
                    neighborCell.LiquidAmount += spreadAmount;
                }
            }

            processedEvents++;
        }

        // ��������� ����� �������� �� ��������
        foreach (var newCell in newWaterCells)
        {
            activeWaterCells[newCell.Key] = newCell.Value;
        }
    }

    /// <summary>
    /// ������� �������� �������� ��� ��������� ���� ������� �� �����������.
    /// </summary>
    /// <returns>����� ��������.</returns>
    private IEnumerable<Vector3Int> GetSpreadDirections()
    {
        return allowDiagonalSpread
            ? new[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right,
                      new Vector3Int(1, 1, 0), new Vector3Int(-1, 1, 0),
                      new Vector3Int(1, -1, 0), new Vector3Int(-1, -1, 0) }
            : new[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
    }

    /// <summary>
    /// ����������� ��������� �������� ���� � ���������.
    /// </summary>
    private struct WaterCell
    {
        public Vector3Int GridPosition { get; } // ������� �� ����
        public float LiquidAmount { get; set; } // ������� ������� ����� � ��������
        public float SpreadPressure { get; set; } // �������� ���� ��� ���������

        public WaterCell(Vector3Int position, float amount)
        {
            GridPosition = position;
            LiquidAmount = amount;
            SpreadPressure = amount;
        }
    }
}
