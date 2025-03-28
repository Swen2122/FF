using UnityEngine;
[CreateAssetMenu(fileName = "New Test Effect", menuName = "Status Effects/Test Effect")]
public class testEffect : BaseStatusEffect
{
    public override void ApplyEffect(GameObject target)
    {
        base.ApplyEffect(target);
        Debug.Log("Test Effect Applied");
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        Debug.Log("Test Effect Removed");
    }
    protected override void EffectTick()
    {
        Debug.Log("Test Effect Tick");
    }
}
