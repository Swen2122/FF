using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Damage : MonoBehaviour
{
    public void Earth(GameObject[] targets, int damage)
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

    public void Fire(GameObject[] targets, int damage)
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

    public void Water(GameObject[] targets, int damage)
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
    public void Wind(GameObject[] targets, int damage)
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
