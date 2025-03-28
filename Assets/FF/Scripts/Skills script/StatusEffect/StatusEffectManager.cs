using UnityEngine;
using System.Collections.Generic;

public class StatusEffectManager : MonoBehaviour
{
    public static StatusEffectManager Instance { get; private set; }

    public StatusEffectSO effectData;
    public static List<BaseStatusEffect> statusEffects = new List<BaseStatusEffect>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }

        if (effectData == null)
        {
            Debug.LogError("StatusEffectManager: effectData не встановлено в інспекторі!");
        }
    }

    public static void AddStatusEffect(Element elementFirst, Element elementSecond)
    {
        if (Instance == null || Instance.effectData == null)
        {
            Debug.LogError("StatusEffectManager: effectData не ініціалізовано!");
            return;
        }

        BaseStatusEffect effect = Instance.effectData.GetStatusEffect(elementFirst, elementSecond);
        if (effect != null)
        {
            statusEffects.Add(effect);
        }
    }

    private void Update()
    {
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            statusEffects[i].UpdateEffect();
            if (!statusEffects[i].isActive)
            {
                statusEffects.RemoveAt(i);
            }
        }
    }
}
