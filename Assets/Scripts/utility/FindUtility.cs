using System.Collections.Generic;
using UnityEngine;

public static class FindUtility
{
    public static HashSet<GameObject> FindEnemy(Collider2D meleeAttackCollider, LayerMask targetLayer)
    {
        // Ініціалізуємо HashSet для зберігання ворогів
        HashSet<GameObject> enemies = new HashSet<GameObject>();

        // Знаходимо всі колайдери в зоні колайдера об'єкта
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(meleeAttackCollider.bounds.center, meleeAttackCollider.bounds.size, 0f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // Якщо колайдер належить об'єкту з необхідним шаром (наприклад, "Enemy")
            if (((1 << hitCollider.gameObject.layer) & targetLayer) != 0)
            {
                // Додаємо об'єкт до HashSet (унікальність забезпечується автоматично)
                enemies.Add(hitCollider.gameObject);
            }
        }

        // Повертаємо HashSet з унікальними ворогами
        return enemies;
    }
}
