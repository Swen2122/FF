using UnityEngine;
public interface IElementalObject
{
    Element CurrentElement { get; }
    GameObject GameObject { get; }
    void OnReact(ElementalReaction reaction, Vector3 position);
}