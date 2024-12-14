using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damage
{
    public static void Earth(GameObject[] targets, int damage)
    {
        foreach (GameObject target in targets) 
        { 
            if (target != null)
            {
                if (target != null && target.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    Debug.Log("Eart");
                    hitTarget.TakeHit(damage);
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
                if (target != null && target.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    Debug.Log("Fire");
                    hitTarget.TakeHit(damage);
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
                if (target != null && target.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    Debug.Log("Water");
                    hitTarget.TakeHit(damage);
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
                if (target != null && target.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    Debug.Log("Wind");
                    hitTarget.TakeHit(damage);
                }
            }
        }
    }
}
