using UnityEngine;

[CreateAssetMenu(fileName = "EffectColor", menuName = "Effect/EffectColor")]
public class EffectColor : ProjectileEffectBase
{
    public Color color;
    public override void OnHit(Collider2D other)
    {
        other.gameObject.GetComponent<Health>().spriteRenderer.color = color;
    }
}
