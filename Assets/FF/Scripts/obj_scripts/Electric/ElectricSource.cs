using System.Collections.Generic;
using UnityEngine;

public class ElectricSource : ICanHit
{
    public float power = 0;
    public float maxPower = 100;
    public SpriteRenderer spriteIndicator;

    List<ElectricWire> conectsWire = new List<ElectricWire>();

    private void FixedUpdate()
    {        
        FindConect();
        DistributePower();
        power = Mathf.Clamp(power, 0, maxPower);//обмеження заряду
    }
    private void FindConect()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var collider in colliders)
        {
            ElectricWire wire = collider.gameObject.GetComponent<ElectricWire>();
            if (wire != null && !conectsWire.Contains(wire))
            {
                conectsWire.Add(wire);
            }
        }
    }
    private void DistributePower()
    {
        conectsWire.RemoveAll(wire => wire == null);
        foreach (var wire in conectsWire)
        {
            if (wire != null && wire.gameObject != null)
            {
                wire.UpdatePower(power);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
    public override void TakeHit(float damage, Element element)
    {

        if(element != Element.Electro) return;
        power += damage;
        //розрахунок % заряду(бажано кудись перенести)
        float percent = power / maxPower;
        Material material = spriteIndicator.material;
        material.SetFloat("_FillAmount", percent);
       
    }
    public override bool IsDestroyed()
    {
        return false;
    }
    protected override void DestroyObject()
    {
        Destroy(gameObject);
    }
}
