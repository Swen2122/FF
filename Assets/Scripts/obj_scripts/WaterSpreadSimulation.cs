using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WaterSystem : MonoBehaviour
{
    [Header("Tilemap References")]
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Simulation Settings")]
    [SerializeField] private float dissipationRate = 0.1f;
    [SerializeField] private float minLiquidAmount = 0.1f;
    [SerializeField] private float pressureDecayRate = 0.8f;
    [SerializeField] private int maxUpdatesPerFrame = 50;
    [SerializeField] private bool allowDiagonalSpread = true;

    // Cached spread directions to avoid GC allocations
    private readonly Vector3Int[] orthogonalDirections = new[]
    {
        Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left
    };

    private readonly Vector3Int[] diagonalDirections = new[]
    {
        new Vector3Int(1, 1, 0), new Vector3Int(1, -1, 0),
        new Vector3Int(-1, 1, 0), new Vector3Int(-1, -1, 0)
    };

    // Object pooling for water cells
    private readonly Stack<WaterCell> cellPool = new Stack<WaterCell>();
    private readonly Dictionary<Vector2Int, WaterCell> activeCells = new Dictionary<Vector2Int, WaterCell>();

    // Spatial partitioning for optimization
    private const int CHUNK_SIZE = 16;
    private readonly Dictionary<Vector2Int, HashSet<WaterCell>> spatialGrid = new Dictionary<Vector2Int, HashSet<WaterCell>>();

    private bool isSimulationRunning;
    private Queue<WaterCell> spreadQueue = new Queue<WaterCell>();

    private void Start()
    {
        ValidateComponents();
        PrewarmPool(100); // Pre-allocate some cells
    }

    private void ValidateComponents()
    {
        if (waterTilemap == null)
        {
            waterTilemap = GetComponent<Tilemap>();
            if (waterTilemap == null)
            {
                throw new MissingComponentException("WaterSystem requires a Tilemap reference!");
            }
        }

        if (waterTile == null)
        {
            throw new MissingReferenceException("Water tile asset is not assigned!");
        }
    }

    private void PrewarmPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            cellPool.Push(new WaterCell());
        }
    }

    public void AddWater(Vector2 worldPosition, float amount, float pressure)
    {
        Vector3Int gridPosition = waterTilemap.WorldToCell(new Vector3(worldPosition.x, worldPosition.y, 0));
        Vector2Int position = new Vector2Int(gridPosition.x, gridPosition.y);

        if (Physics2D.OverlapPoint(worldPosition, obstacleLayer)) return;

        if (!activeCells.TryGetValue(position, out WaterCell cell))
        {
            cell = GetOrCreateCell(position, amount, pressure);
            AddToSpatialGrid(cell);
            waterTilemap.SetTile(gridPosition, waterTile);
        }
        else
        {
            cell.AddLiquid(amount);
            cell.UpdatePressure(pressure);
        }

        if (!isSimulationRunning)
        {
            StartSimulation();
        }
    }

    private void StartSimulation()
    {
        if (!isSimulationRunning)
        {
            isSimulationRunning = true;
            InvokeRepeating(nameof(UpdateSimulation), 0f, 0.02f); // 50 times per second
        }
    }

    private void StopSimulation()
    {
        if (isSimulationRunning && activeCells.Count == 0)
        {
            isSimulationRunning = false;
            CancelInvoke(nameof(UpdateSimulation));
        }
    }

    private void UpdateSimulation()
    {
        int updates = 0;
        float deltaTime = Time.deltaTime;

        foreach (var cell in activeCells.Values)
        {
            if (updates >= maxUpdatesPerFrame) break;

            ProcessCell(cell, deltaTime);
            updates++;
        }

        // Process spread queue
        while (spreadQueue.Count > 0 && updates < maxUpdatesPerFrame)
        {
            var cell = spreadQueue.Dequeue();
            SpreadWater(cell);
            updates++;
        }

        CleanupInactiveCells();
    }

    private void ProcessCell(WaterCell cell, float deltaTime)
    {
        cell.ApplyDissipation(dissipationRate * deltaTime);
        cell.DecayPressure(pressureDecayRate * deltaTime);

        if (cell.Amount > minLiquidAmount)
        {
            spreadQueue.Enqueue(cell);
        }
    }

    private void SpreadWater(WaterCell cell)
    {
        // Spread to orthogonal neighbors
        foreach (var dir in orthogonalDirections)
        {
            TrySpreadToNeighbor(cell, dir, 1f);
        }

        // Spread to diagonal neighbors if allowed
        if (allowDiagonalSpread)
        {
            foreach (var dir in diagonalDirections)
            {
                TrySpreadToNeighbor(cell, dir, 0.7f); // Reduced spread for diagonals
            }
        }
    }

    private void TrySpreadToNeighbor(WaterCell source, Vector3Int direction, float spreadFactor)
    {
        Vector2Int neighborPos = source.Position + new Vector2Int(direction.x, direction.y);

        if (Physics2D.OverlapPoint(GetWorldPosition(neighborPos), obstacleLayer))
            return;

        float spreadAmount = source.GetSpreadAmount() * spreadFactor;

        if (spreadAmount < minLiquidAmount)
            return;

        if (!activeCells.TryGetValue(neighborPos, out WaterCell neighbor))
        {
            neighbor = GetOrCreateCell(neighborPos, spreadAmount, source.Pressure * spreadFactor);
            AddToSpatialGrid(neighbor);
            waterTilemap.SetTile(new Vector3Int(neighborPos.x, neighborPos.y, 0), waterTile);
        }
        else
        {
            neighbor.AddLiquid(spreadAmount);
            neighbor.UpdatePressure(source.Pressure * spreadFactor);
        }
    }

    private Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return waterTilemap.GetCellCenterWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0));
    }

    private void CleanupInactiveCells()
    {
        var toRemove = new List<Vector2Int>();

        foreach (var kvp in activeCells)
        {
            if (kvp.Value.Amount <= minLiquidAmount)
            {
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var pos in toRemove)
        {
            RemoveCell(pos);
        }

        if (activeCells.Count == 0)
        {
            StopSimulation();
        }
    }

    private WaterCell GetOrCreateCell(Vector2Int position, float amount, float pressure)
    {
        WaterCell cell;
        if (cellPool.Count > 0)
        {
            cell = cellPool.Pop();
            cell.Reset(position, amount, pressure);
        }
        else
        {
            cell = new WaterCell(position, amount, pressure);
        }

        activeCells[position] = cell;
        return cell;
    }

    private void RemoveCell(Vector2Int position)
    {
        if (activeCells.TryGetValue(position, out WaterCell cell))
        {
            RemoveFromSpatialGrid(cell);
            waterTilemap.SetTile(new Vector3Int(position.x, position.y, 0), null);
            cellPool.Push(cell);
            activeCells.Remove(position);
        }
    }

    private void AddToSpatialGrid(WaterCell cell)
    {
        Vector2Int chunk = GetChunkPosition(cell.Position);
        if (!spatialGrid.TryGetValue(chunk, out var cells))
        {
            cells = new HashSet<WaterCell>();
            spatialGrid[chunk] = cells;
        }
        cells.Add(cell);
    }

    private void RemoveFromSpatialGrid(WaterCell cell)
    {
        Vector2Int chunk = GetChunkPosition(cell.Position);
        if (spatialGrid.TryGetValue(chunk, out var cells))
        {
            cells.Remove(cell);
            if (cells.Count == 0)
            {
                spatialGrid.Remove(chunk);
            }
        }
    }

    private Vector2Int GetChunkPosition(Vector2Int position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / (float)CHUNK_SIZE),
            Mathf.FloorToInt(position.y / (float)CHUNK_SIZE)
        );
    }

    private class WaterCell
    {
        public Vector2Int Position { get; private set; }
        public float Amount { get; private set; }
        public float Pressure { get; private set; }

        public WaterCell() : this(Vector2Int.zero, 0, 0) { }

        public WaterCell(Vector2Int position, float amount, float pressure)
        {
            Reset(position, amount, pressure);
        }

        public void Reset(Vector2Int position, float amount, float pressure)
        {
            Position = position;
            Amount = amount;
            Pressure = pressure;
        }

        public void AddLiquid(float amount)
        {
            Amount += amount;
        }

        public void ApplyDissipation(float rate)
        {
            Amount = Mathf.Max(0, Amount - rate);
        }

        public void UpdatePressure(float newPressure)
        {
            Pressure = Mathf.Max(Pressure, newPressure);
        }

        public void DecayPressure(float rate)
        {
            Pressure *= rate;
        }

        public float GetSpreadAmount()
        {
            return Amount * Pressure * 0.25f; // Spread 25% of current amount
        }
    }
}