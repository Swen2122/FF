using UnityEngine;

public interface IStatusEffect
{
    void Apply(GameObject target);
    void Update();
    void Remove();
    bool IsFinished { get; }
    float Duration { get; }
    string EffectId { get; }
}
