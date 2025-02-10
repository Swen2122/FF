using UnityEngine;

[CreateAssetMenu(fileName = "projectileEffectBase", menuName = "Effect/projectileEffectBase")]
public abstract class ProjectileEffectBase : ScriptableObject
{
    abstract public void OnHit(Collider2D other);
}
