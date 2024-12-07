using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Skill : MonoBehaviour
{
    public EnemyShoot shoot;
    [SerializeReference] private GameObject bolt_bullet;
    [SerializeReference] private GameObject HEAD_bullet;
    public void BoltShot(Transform target,float speed, int damage)
    {
        shoot.Bullet(bolt_bullet, target, speed, damage, 1f);
    }
    public void HeadThrow(Transform target, float speed, int damage)
    {
        shoot.Bullet(HEAD_bullet, target, speed, damage, 3f);
    }
    private void Charge()
    {

    }
}
