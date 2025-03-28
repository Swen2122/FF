using UnityEngine;

[CreateAssetMenu(fileName = "NewHealBurst", menuName = "Skills/Burst/HealBurst")]
public class HealBurst : AnBurstSO
{
    public LayerMask enemyLayer;
    public float healAmount;
    public float energyDamage;
    public override void UseBurst(Transform playesPosition)
    {
        base.UseBurst(playesPosition);
        Debug.Log($"Water Burst: {element}");
    }
    protected override void ApplyEffect()
    {
        Debug.Log("Applying effect");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ownerTransform.position, range, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.TryGetComponent<ICanHit>(out var health))
            {
                health.TakeEnergy(energyDamage, element);
            }
        }
        if(ownerTransform.TryGetComponent<ICanHit>(out var playerHealth))
        {
            playerHealth.TakeHit(healAmount*-1, element);
        }
    }
}
