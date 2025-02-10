using UnityEngine;
public interface IElementalObject
{
    Element CurrentElement { get; }
    
    GameObject GameObject { get; }
    bool CanTriggerReaction { get; }
    void OnReact(ElementalReaction reaction = null, Vector3 position = default);
}