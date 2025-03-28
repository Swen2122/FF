using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusReaction", menuName = "Element/StatusReaction")]
public class StatusReaction : ScriptableObject
{
    [SerializeField] private Element firstElement;
    [SerializeField] private Element secondElement;
    [SerializeField] private BaseStatusEffect reactionEffect;

    public Element FirstElement => firstElement;
    public Element SecondElement => secondElement;
    public BaseStatusEffect Effect => reactionEffect;
}
