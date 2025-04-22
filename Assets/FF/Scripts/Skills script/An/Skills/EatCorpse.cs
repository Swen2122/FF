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
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 1.5f, targetLayerMask);
        Debug.Log(collider);
        if (collider != null)
        {
            Health enemy = collider.GetComponent<Health>();
            Debug.Log(enemy);
            if (enemy != null && enemy.healthState == HealthState.corpse)
            {
                float healAmount = enemy.maxHP/10f; 
                playerHealth.TakeHit(-healAmount, Element.Water);
                Debug.Log($"Healed for {healAmount} HP"); 
                Destroy(enemy.gameObject);
            } else Debug.Log("Not a corpse or no health component found.");
        }
    }
    private void OnDrawGizmos()
    {
        // Встановлюємо колір гізмо
        Gizmos.color = Color.red;
        
        // Малюємо коло радіусом 3 одиниці
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
    
}
