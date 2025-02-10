using System.Collections.Generic;
using UnityEngine;

public class ElectricWire : MonoBehaviour, IElectric
{
    float power = 0;
    private List<IElectric> conects = new List<IElectric>();
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    public void UpdatePower(float power)
    {        
        FindConect();
        RevicePower(power); 
        if(power > 0) spriteRenderer.enabled = true;
        else spriteRenderer.enabled = false;
    }
    public void RevicePower(float power)
    {
        if(this.power == power)return;
        this.power = power;
        foreach (var conect in conects)
        {
            conect.UpdatePower(power);
        }
    }
    public void FindConect()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<IElectric>() != null && !conects.Contains(collider.gameObject.GetComponent<IElectric>()))
            {
                conects.Add(collider.gameObject.GetComponent<IElectric>());
            }
        }
    }
}
