using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Damage : MonoBehaviour
{
    public void atk(GameObject[] targets, int damage)
    {
        foreach(GameObject target in targets) 
        { 
            if (target != null)
            {
                getDMG health;
                if (target.TryGetComponent<getDMG>(out health))
                {
                    health.TakeDMG(damage);
                }
            }
        }
    }
}
