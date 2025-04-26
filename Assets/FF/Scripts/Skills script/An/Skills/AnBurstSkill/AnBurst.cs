using UnityEngine;

public class AnBurst : BaseSkills
{
    public BurstDataBase burstData;
    [SerializeField]protected AnBurstSO burst;
    [SerializeField] protected Element_use elementController;
    protected override void UseSkill()
    {
        Element element = elementController.currentElement;
        burst = burstData.GetBurst(element);
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
