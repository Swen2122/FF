using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Skill : MonoBehaviour
{
    public Shoot shoot;
    [SerializeReference] private GameObject Water_bullet;
    [SerializeReference] private GameObject Earth_bullet;
    public void EarthM2(float speed, int damage)
    {
        shoot.bullet(Earth_bullet, speed, damage, 1.5f);
    }
    public void FireM2(GameObject[] targets, int damage)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                getDMG health;
                if (target.TryGetComponent<getDMG>(out health))
                {
                    Debug.Log("FireM2");
                    health.TakeDMG(damage);
                }
            }
        }
    }
    public void WaterM2(float speed, int damage)
    {
        shoot.bullet(Water_bullet, speed, damage, 3f);
    }
    public void WindM2(GameObject[] targets, int damage)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                getDMG health;
                if (target.TryGetComponent<getDMG>(out health))
                {
                    Debug.Log("windM2");
                    health.TakeDMG(damage);
                }
            }
        }
    }
}
