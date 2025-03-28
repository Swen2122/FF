using UnityEngine;

[CreateAssetMenu(fileName = "NewEffectBurst", menuName = "Skills/Burst/EffectBurst")]
public class EffectBurst : AnBurstSO
{
    protected override void ApplyEffect()
    {
        Debug.Log("Applying effect");
    }
}
