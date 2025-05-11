using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class FrezeScript : TargetedSkill
{
    public Tilemap[] tilemaps; // Тайлмапа, на яким потрібно заморожувати тайли
    public Tilemap frozenTilemap; // Тайлмапа, на якому будуть заморожені тайли
    public TileBase frozenTile; // Тайл, який буде використовуватися для заморожених тайлів
    protected override void UseSkillAtPosition(Vector3 targetPosition)
    {
        if (!CanUseSkill()) return;           
        // Отримуємо тайли в області навколо цілі
        foreach (var tilemap in tilemaps)
        {
            // Отримуємо тайли в області навколо цілі для кожного tilemap
            List<Vector3Int> tilesInArea = TileFinder.Instance.GetTilesInArea(tilemap, targetPosition, new Vector2(3, 3));
            // Заморожуємо тайли
            foreach (Vector3Int tilePosition in tilesInArea)
            {
                frozenTilemap.SetTile(tilePosition, frozenTile);
                tilemap.SetTile(tilePosition, null); // Видаляємо тайл з основного tilemap
            }
        }

    }
}
