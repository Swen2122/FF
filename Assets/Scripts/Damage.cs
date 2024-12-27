using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damage
{
    public static void Earth(GameObject[] targets, float damage)
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

    public static void Fire(GameObject[] targets, float damage)
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

    public static void Water(GameObject[] targets, float damage)
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
    public static void Wind(GameObject[] targets, float damage)
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
    public static void Ice(GameObject[] targets, float damage)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                if (target != null && target.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    Debug.Log("Ice");
                    hitTarget.TakeHit(damage);
                }
            }
        }
    }
    public static void Electro(GameObject[] targets, float damage)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                if (target != null && target.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    Debug.Log("Electro");
                    hitTarget.TakeHit(damage);
                }
            }
        }
    }
}
