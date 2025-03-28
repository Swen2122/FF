using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damage
{
    public static void ApplyDamage(GameObject[] targets, float damage, Element damageType)
    {
        foreach (GameObject target in targets) 
        { 
            if (target != null)
            {
                if (target != null && target.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    Debug.Log(damageType);
                    hitTarget.TakeHit(damage, damageType);
                }
            }
        }
    }
}
