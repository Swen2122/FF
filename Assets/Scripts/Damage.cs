using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damage
{
    public static void Earth(GameObject[] targets, int damage)
    {
        foreach(GameObject target in targets) 
        { 
            if (target != null)
            {
                getDMG health;
                if (target.TryGetComponent<getDMG>(out health))
                {
                    Debug.Log("Eart");
                    health.TakeDMG(damage);
                }
            }
        }
    }

    public static void Fire(GameObject[] targets, int damage)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                getDMG health;
                if (target.TryGetComponent<getDMG>(out health))
                {
                    Debug.Log("Fire");
                    health.TakeDMG(damage);
                }
            }
        }
    }

    public static void Water(GameObject[] targets, int damage)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                getDMG health;
                if (target.TryGetComponent<getDMG>(out health))
                {
                    Debug.Log("Water");
                    health.TakeDMG(damage);
                }
            }
        }
    }
    public static void Wind(GameObject[] targets, int damage)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                getDMG health;
                if (target.TryGetComponent<getDMG>(out health))
                {
                    Debug.Log("Wind");
                    health.TakeDMG(damage);
                }
            }
        }
    }
}
