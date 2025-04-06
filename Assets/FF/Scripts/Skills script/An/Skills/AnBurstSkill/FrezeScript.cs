using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class FrezeScript : TargetedSkill
{
    public Tilemap tilemap; // Тайлмапа, на яким потрібно заморожувати тайли
    public Tilemap frozenTilemap; // Тайлмапа, на якому будуть заморожені тайли
    public Tile frozenTile; // Тайл, який буде використовуватися для заморожених тайлів
    protected override void UseSkillAtPosition(Vector2 targetPosition)
    {
        if (!CanUseSkill()) return;           
        // Отримуємо тайли в області навколо цілі
        List<Vector3Int> tilesInArea = TileFinder.Instance.GetTilesInArea(tilemap, targetPosition, new Vector2(3, 3));
        // Заморожуємо тайли
        foreach (Vector3Int tilePosition in tilesInArea)
        {
            frozenTilemap.SetTile(tilePosition, frozenTile);
            tilemap.SetTile(tilePosition, null); // Видаляємо тайл з основного тайлмапа
        }

    }
}
