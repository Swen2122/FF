using UnityEngine;
using System.Collections;

public class AnBurst : BaseSkills
{
    public BurstDataBase burstData;
    public Health playerHealth;
    [SerializeField]protected AnBurstSO burst;
    protected bool isActive = true;
    protected override void UseSkill()
    {
        Element elementBurst = element.currentElement;
        burst = burstData.GetBurst(elementBurst);
        burst.UseBurst(transform);    
        if(burst.tickInterval > 0)      
            {
                StartCoroutine(BurstCoroutine(burst.duration, burst.tickInterval));
                Debug.Log("Bursting");
            }else
            {
                burst.ApplyEffect(GetEnemiesInRange(), playerHealth);
                Debug.Log("Burst false");
                burst = null;
            } 
    }
    private IEnumerator BurstCoroutine(float duration, float tickInterval)
    {
        if (burst == null) yield break;
        
        float durationStartTime = 0f;
        while (durationStartTime < duration && burst != null)
        {
            yield return new WaitForSeconds(tickInterval);
            durationStartTime += tickInterval;
            
            if (burst != null)
            {
                burst.ApplyEffect(GetEnemiesInRange(), playerHealth);
            }
        }
        burst = null;
    }

    private Collider2D[] GetEnemiesInRange()
    {
        if (burst == null) return new Collider2D[0];
        
        return Physics2D.OverlapCircleAll(transform.position, burst.range, burst.targetLayer);
    } 
   
}
