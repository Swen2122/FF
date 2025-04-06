using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class TileFinder : MonoBehaviour
{
   public static TileFinder Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
       public List<Vector3Int> GetTilesInArea(Tilemap tilemap, Vector2 center, Vector2 size)
    {
        List<Vector3Int> tilePositions = new List<Vector3Int>();
        
        // Конвертуємо світові координати в координати тайлмапи
        Vector3Int centerCell = tilemap.WorldToCell(new Vector3(center.x, center.y, 0));
        
        // Розраховуємо межі пошуку
        int halfWidth = Mathf.CeilToInt(size.x / 2f);
        int halfHeight = Mathf.CeilToInt(size.y / 2f);
        
        BoundsInt searchBounds = new BoundsInt(
            centerCell.x - halfWidth,
            centerCell.y - halfHeight,
            0,
            halfWidth * 2,
            halfHeight * 2,
            1
        );

        // Перевіряємо кожен тайл в межах
        for (int x = searchBounds.xMin; x < searchBounds.xMax; x++)
        {
            for (int y = searchBounds.yMin; y < searchBounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                
                // Перевіряємо чи є тайл на цій позиції
                if (tilemap.HasTile(tilePosition))
                {
                    tilePositions.Add(tilePosition);
                    
                    // Візуальний дебаг
                    #if UNITY_EDITOR
                    Debug.DrawLine(
                        tilemap.GetCellCenterWorld(tilePosition),
                        tilemap.GetCellCenterWorld(tilePosition) + Vector3.up * 0.5f,
                        Color.yellow,
                        2f
                    );
                    #endif
                }
            }
        }

        return tilePositions;
    }
}
