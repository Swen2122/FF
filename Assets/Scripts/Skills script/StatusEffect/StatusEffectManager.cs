using UnityEngine;
using System.Collections.Generic;
using System;
public class StatusEffectManager : MonoBehaviour
{
    private Dictionary<string, IStatusEffect> activeEffects = new Dictionary<string, IStatusEffect>();
    private Dictionary<string, ParticleSystem> effectParticles = new Dictionary<string, ParticleSystem>();

    public event Action<string> OnEffectAdded;
    public event Action<string> OnEffectRemoved;

    private void Update()
    {
        List<string> effectsToRemove = new List<string>();

        foreach (var effect in activeEffects.Values)
        {
            effect.Update();
            if (effect.IsFinished)
            {
                effectsToRemove.Add(effect.EffectId);
            }
        }

        foreach (var effectId in effectsToRemove)
        {
            RemoveEffect(effectId);
        }
    }

    public void AddEffect(IStatusEffect effect)
    {
        string effectId = effect.EffectId;

        // якщо ефект вже ≥снуЇ, оновлюЇмо його тривал≥сть
        if (activeEffects.ContainsKey(effectId))
        {
            RemoveEffect(effectId);
        }

        effect.Apply(gameObject);
        activeEffects.Add(effectId, effect);
        OnEffectAdded?.Invoke(effectId);
    }

    public void RemoveEffect(string effectId)
    {
        if (activeEffects.TryGetValue(effectId, out var effect))
        {
            effect.Remove();
            activeEffects.Remove(effectId);
            OnEffectRemoved?.Invoke(effectId);
        }
    }

    public bool HasEffect(string effectId)
    {
        return activeEffects.ContainsKey(effectId);
    }
}
