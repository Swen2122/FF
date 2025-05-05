using UnityEngine;

public class EatCorpse : BaseSkills
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private LayerMask targetLayerMask;
    protected override void UseSkill()
    {
        Debug.Log("EatCorpse is used");
        EatTarget();
    }
    private void EatTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f, targetLayerMask);
        Health enemyHealth = FindFood(colliders);
        if (enemyHealth != null)
        {
            float healAmount = enemyHealth.maxHP/10f; 
            playerHealth.TakeHit(-healAmount, Element.Water);
            Debug.Log($"Healed for {healAmount} HP"); 
            Destroy(enemyHealth.gameObject);
        }
    }
    private Health FindFood(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<Health>(out Health enemyHealth) && enemyHealth.healthState == HealthState.corpse)
            {
                return enemyHealth;
            }
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        // Встановлюємо колір гізмо
        Gizmos.color = Color.red;
        
        // Малюємо коло радіусом 3 одиниці
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
    
}
