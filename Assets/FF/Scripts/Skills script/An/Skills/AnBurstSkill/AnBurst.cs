using UnityEngine;

public class AnBurst : BaseSkills
{
    public BurstDataBase burstData;
    [SerializeField]protected AnBurstSO burst;
    protected override void UseSkill()
    {
        Element elementBurst = element.currentElement;
        burst = burstData.GetBurst(elementBurst);
        burst.UseBurst(transform);     
    }
    protected void Update()
    {
        if(burst != null)
        {
            Debug.Log("Burst not null");
            if(burst.IsBursting() && burst.tickInterval > 0)      
            {
                burst.UpdateBurst(); 
                Debug.Log("Bursting");
            }
            else
            {
                Debug.Log("Burst false");
                burst = null;
            }
        }
    }
}
