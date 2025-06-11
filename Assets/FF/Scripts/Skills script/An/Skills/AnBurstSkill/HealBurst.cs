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
    public override void ApplyEffect(Collider2D[] enemys, Health playerHealth)
    {
        Debug.Log("Applying effect");
        foreach (var enemy in enemys)
        {
            if(enemy.TryGetComponent<ICanHit>(out var health))
            {
                health.TakeEnergy(energyDamage, element);
            }
        }
        if(playerHealth)
        {
            playerHealth.TakeHit(healAmount*-1, element);
        }
    }
}
