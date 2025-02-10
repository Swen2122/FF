using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class GradualWaterSystem : MonoBehaviour
{
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private LayerMask blockingLayer;

    [Header("Spread Settings")]
    [SerializeField] private float spreadInterval = 0.2f;
    [SerializeField] private float costReductionRate = 1f;

    private Queue<WaterSpread> spreadQueue = new Queue<WaterSpread>();
    private bool isProcessing = false;

    public void InitiateWaterSpread(Vector2 worldPosition, float initialCost)
    {
        Vector3Int cellPosition = waterTilemap.WorldToCell(worldPosition);
        Vector2Int gridPosition = new Vector2Int(cellPosition.x, cellPosition.y);

        if (!Physics2D.OverlapPoint(worldPosition, blockingLayer))
        {
            spreadQueue.Enqueue(new WaterSpread(gridPosition, initialCost));

            if (!isProcessing)
                StartCoroutine(ProcessWaterSpread());
        }
    }

    private IEnumerator ProcessWaterSpread()
    {
        isProcessing = true;

        while (spreadQueue.Count > 0)
        {
            WaterSpread currentSpread = spreadQueue.Dequeue();

            if (currentSpread.Cost > 0)
                SpreadWaterCell(currentSpread);

            yield return new WaitForSeconds(spreadInterval);
        }

        isProcessing = false;
    }

    private void SpreadWaterCell(WaterSpread waterSpread)
    {
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = waterSpread.Position + dir;
            Vector3Int cellPos = new Vector3Int(newPos.x, newPos.y, 0);
            Vector2 worldPos = waterTilemap.GetCellCenterWorld(cellPos);

            if (!Physics2D.OverlapPoint(worldPos, blockingLayer))
            {
                float newCost = waterSpread.Cost - costReductionRate;

                if (newCost > 0)
                {
                    waterTilemap.SetTile(cellPos, waterTile);
                    spreadQueue.Enqueue(new WaterSpread(newPos, newCost));
                }
            }
        }
    }

    private class WaterSpread
    {
        public Vector2Int Position { get; private set; }
        public float Cost { get; private set; }

        public WaterSpread(Vector2Int position, float cost)
        {
            Position = position;
            Cost = cost;
        }
    }
}