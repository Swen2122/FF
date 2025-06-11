using UnityEngine;

[CreateAssetMenu(fileName = "NewEffectBurst", menuName = "Skills/Burst/EffectBurst")]
public class EffectBurst : AnBurstSO
{
    public override void ApplyEffect(Collider2D[] enemys, Health playerHealth)
    {
       foreach (var enemy in enemys)
        {
            if(enemy.TryGetComponent<Rigidbody2D>(out var rb) && enemy.gameObject.layer != LayerMask.NameToLayer("PlayerProjectile"))
            {
                Vector3 direction = (enemy.transform.position - playerHealth.transform.position).normalized;
                rb.AddForce(direction * 10f, ForceMode2D.Impulse);
            }
        }
    }
}
