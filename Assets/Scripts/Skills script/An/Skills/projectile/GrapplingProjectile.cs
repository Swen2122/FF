using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplingProjectile : BaseProjectile
{
    [SerializeField] private float pullForce = 10f;
    [SerializeField] private LayerMask pullableLayer;
    private List<Rigidbody2D> pulledTargets = new List<Rigidbody2D>();

    protected override void OnHit(Collider2D other)
    {
        if (other.TryGetComponent<Rigidbody2D>(out var rb) &&
            ((1 << other.gameObject.layer) & pullableLayer) != 0)
        {
            if (!pulledTargets.Contains(rb))
            {
                pulledTargets.Add(rb);
                if (other.TryGetComponent<ICanHit>(out var target))
                {
                    target.TakeHit(damage, Element.None);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (var target in pulledTargets.ToList())
        {
            if (target != null)
            {
                Vector2 pullDirection = transform.position - target.transform.position;
                target.AddForce(pullDirection.normalized * pullForce);
            }
            else
            {
                pulledTargets.Remove(target);
            }
        }
    }

    protected override void OnProjectileReachedTarget()
    {
        pulledTargets.Clear();
        Destroy(gameObject);
    }
}

