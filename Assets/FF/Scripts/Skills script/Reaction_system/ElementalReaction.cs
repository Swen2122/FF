using UnityEngine;

[CreateAssetMenu(fileName = "NewElementalReaction", menuName = "Element/ElementalReaction")]
public class ElementalReaction : ScriptableObject
{
    [System.Serializable]
    public class ReactionEffect
    {
        public string reactionName;
        public LayerMask targetLeyer;
        public GameObject effectPrefab;
        public float damage;
        public float radius;
        public float energy; // ������� ������ �������
        public float energyPerTick; // ������, ��� ����������� �� ��
        public float tickInterval; // �������� �� �����
        public ParticleSystem particles;
        public AudioClip sound;
        public ReactionBehavior behavior;
        public BaseStatusEffect statusEffect;
        public bool allowMultipleReactions = false;
    }

    public enum ReactionBehavior
    {
        SpawnEffect,
        ApplyStatusEffect,
        ChangeEnvironment,
        ChainReaction
    }

    [SerializeField] private Element firstElement;
    [SerializeField] private Element secondElement;
    [SerializeField] private ReactionEffect reactionEffect;

    public Element FirstElement => firstElement;
    public Element SecondElement => secondElement;
    public ReactionEffect Effect => reactionEffect;
}
