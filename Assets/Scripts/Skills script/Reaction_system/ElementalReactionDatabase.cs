using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewReactionDatabase", menuName = "Element/Reaction Database")]
public class ElementalReactionDatabase : ScriptableObject
{
    [SerializeField] private List<ElementalReaction> reactions;
    private Dictionary<(Element, Element), ElementalReaction> reactionLookup;

    private void OnEnable()
    {
        InitializeLookup();
    }

    private void InitializeLookup()
    {
        reactionLookup = new Dictionary<(Element, Element), ElementalReaction>();
        foreach (var reaction in reactions)
        {
            reactionLookup[(reaction.FirstElement, reaction.SecondElement)] = reaction;
            reactionLookup[(reaction.SecondElement, reaction.FirstElement)] = reaction;
        }
    }

    public ElementalReaction GetReaction(Element first, Element second)
    {
        return reactionLookup.TryGetValue((first, second), out var reaction) ? reaction : null;
    }
}