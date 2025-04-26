using UnityEngine;

public class ReactionItem : MonoBehaviour
{
    public Element element;
    public bool isDestroyedOnReaction = true;

    public void StartReaction(Element otherElement, GameObject target, Vector3 position)
    {
        ElementalReactionHandler.Instance.TriggerReaction(element, otherElement, gameObject, target, position);
        if(isDestroyedOnReaction) Destroy(gameObject);      
    }
}
