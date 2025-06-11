using UnityEngine;

public abstract class AnBurstSO : ScriptableObject
{
    public float range;
    public LayerMask targetLayer;
    public float damage;
    public float duration;
    public float tickInterval;
    public Element element;
    public virtual void UseBurst(Transform playerPosition)
    {
        Collider2D[] hitTargetss = Physics2D.OverlapCircleAll(playerPosition.position, range, targetLayer);
        foreach (Collider2D target in hitTargetss)
        {
            var reactionItem = target.GetComponent<ReactionItem>();
            if (reactionItem != null)
            {
                reactionItem.StartReaction(element, target.gameObject, target.transform.position);
            }
        }
    }
    public abstract void ApplyEffect(Collider2D[] enemys, Health playerHealth);
}
