using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "StatusEffectDataBase", menuName = "Status Effects/StatusEffectDataBase")]
public class StatusEffectSO : ScriptableObject
{
   [System.Serializable]
    public class StatusEffect
    {
         public BaseStatusEffect effect;
         [Header("Effect requirements")]
         public Element elementFirst, elementSecond;
         public float energyFirst, energySecond;
    }
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public BaseStatusEffect GetStatusEffect(Element elementFirst, Element elementSecond)
    {
        foreach (var effect in statusEffects)
        {
            if ((effect.elementFirst == elementFirst && effect.elementSecond == elementSecond) || (effect.elementFirst == elementSecond && effect.elementSecond == elementFirst))
            {
                Debug.Log("Status effect found for elements: " + elementFirst + " and " + elementSecond);
                return effect.effect;
            }            
        }
        Debug.Log("No status effect found for elements: " + elementFirst + " and " + elementSecond);
        return null;
    }
    
}
