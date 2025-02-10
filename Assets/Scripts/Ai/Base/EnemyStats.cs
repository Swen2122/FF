using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "AI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Base Stats")]
    public float maxChaseDistance = 10f;
    public float attackRange = 1.5f;
    public float updatePathInterval = 0.5f;

    [Header("Attack Stats")]
    public int damage;
    public float attackCooldown = 1f;
    public Element enemyElement;

    [Header("Archer Specific")]
    public float minDistance = 4f;
    public float shotCooldown = 3f;
}