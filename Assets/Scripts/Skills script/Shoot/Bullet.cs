using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour, IBulletBehavior
{
    [SerializeField] private LayerMask targets;
    private int bulletDamage;
    private Sequence moveSequence;

    public void SetBulletProperties(int damage)
    {
        bulletDamage = damage;
    }

    public void SetSequence(Sequence sequence)
    {
        moveSequence = sequence;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targets) != 0)
        {
            getDMG target_hp = other.gameObject.GetComponent<getDMG>();

            if (target_hp != null)
            {
                target_hp.TakeDMG(bulletDamage);
            }
            if (moveSequence != null)
            {
                moveSequence.Kill();
                Destroy(gameObject);
            }
        }

    }
}
