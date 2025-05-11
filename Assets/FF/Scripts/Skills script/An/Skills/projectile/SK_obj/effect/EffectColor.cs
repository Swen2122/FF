using UnityEngine;

[CreateAssetMenu(fileName = "EffectColor", menuName = "Effect/EffectColor")]
public class EffectColor : ProjectileEffectBase
{
    public Color color;
    public override void OnHit(Collider2D other)
    {
        var health = other?.gameObject.GetComponent<Health>();
        if (health?.spriteRenderer != null)health.spriteRenderer.color = color;
    }
}
