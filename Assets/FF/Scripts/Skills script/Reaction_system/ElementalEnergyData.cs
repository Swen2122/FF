using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementalEnergyData", menuName = "Element/Reaction Energy Data")]
public class ElementalEnergyData : ScriptableObject
{
    [System.Serializable]
    public class ElementInteraction
    {
        public Element interactingElement;
        public float energyModifier; // >1 посилює, <1 послаблює, <0 зменшує енергію
        public bool canDisrupt; // чи може перервати реакцію
    }

    public Element reactionElement; // Стихія реакції
    public float baseEnergyPerTick = 1f;
    public float maxEnergy = 100f;
    public List<ElementInteraction> elementInteractions;

    public float GetInteractionModifier(Element element)
    {
        var interaction = elementInteractions.Find(x => x.interactingElement == element);
        return interaction?.energyModifier ?? 1f;
    }
}