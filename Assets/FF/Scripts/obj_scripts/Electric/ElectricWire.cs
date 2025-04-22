using System.Collections.Generic;
using UnityEngine;

public class ElectricWire : MonoBehaviour, IElectric
{
    public float power = 0;
    private List<IElectric> conects = new List<IElectric>();
    private SpriteRenderer spriteRenderer;
    private float lastUpdateTime;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    public void UpdatePower(float power)
    {
        if (Time.time - lastUpdateTime > 1f)
        {
            this.power = 0;
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
        }

        lastUpdateTime = Time.time;
        FindConect();
        RevicePower(power);

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = power > 0;
        }
    }
    public void RevicePower(float power)
    {
        if(this.power == power)return;
        this.power = power;
        conects.RemoveAll(conect => conect == null);
        foreach (var conect in conects)
        {
            conect.UpdatePower(power); 
        }
    }
    public void FindConect()
    {
        conects.RemoveAll(conect => conect == null || (conect as MonoBehaviour)?.gameObject == null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var collider in colliders)
        {
            var electric = collider.gameObject.GetComponent<IElectric>();
            if (electric != null && !conects.Contains(electric))
            {
                conects.Add(electric);
            }
        }
    }
    private IElectric CheckPower()
    {
        foreach (var conect in conects)
        {
            if (conect != null) return conect;
        }
        return null;
    }
}
