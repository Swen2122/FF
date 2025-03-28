using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Skill : MonoBehaviour
{
    public Shoot shoot;
    [SerializeField] private GameObject Water_bullet;
    [SerializeField] private GameObject Earth_bullet;
    [SerializeField] private WaterGrab WaterGrab;
    public void EarthM2(float speed, int damage)
    {
        shoot.Bullet(Earth_bullet, speed, damage, 1.5f);
    }
    public void FireM2(GameObject[] targets, int damage)
    {
        
    }
    public void WaterM2()
    {
        WaterGrab.TryCaptureTarget();
    }
    public void WindM2(GameObject[] targets, int damage)
    {
        
    }
}
