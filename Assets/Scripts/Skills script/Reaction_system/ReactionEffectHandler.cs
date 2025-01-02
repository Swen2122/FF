using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementalReactionHandler : MonoBehaviour
{
    [SerializeField] private ElementalReactionDatabase reactionDatabase;

    // ������� ��� ���������� �������� ������� ��� ������� ��'����
    private Dictionary<GameObject, HashSet<(Element, Element)>> activeReactionsPerObject
        = new Dictionary<GameObject, HashSet<(Element, Element)>>();

    // ������ ������������ �������
    public void TriggerReaction(Element first, Element second, GameObject target, Vector3 position)
    {
        var reaction = reactionDatabase.GetReaction(first, second);
        if (reaction == null || target == null) return;

        var reactionKey = (first, second);

        // ���������� �� ����� �������� ���� �������
        if (!CanTriggerReaction(target, reactionKey, reaction.Effect.allowMultipleReactions))
            return;

        // ������ ������� �� ��������
        AddActiveReaction(target, reactionKey);

        // ���������� ������� ������� �� �� ����
        ProcessReaction(reaction.Effect, target, position);

        // ��������� �������� ��� �������� �������
        StartCoroutine(ClearActiveReaction(target, reactionKey, reaction.Effect.energy));
    }

    // �������� ��������� ��������� ���� �������
    private bool CanTriggerReaction(GameObject target, (Element, Element) reactionKey, bool allowMultiple)
    {
        if (!activeReactionsPerObject.ContainsKey(target))
            return true;

        return allowMultiple || !activeReactionsPerObject[target].Contains(reactionKey);
    }

    // ��������� ������� �� ��������
    private void AddActiveReaction(GameObject target, (Element, Element) reactionKey)
    {
        if (!activeReactionsPerObject.ContainsKey(target))
            activeReactionsPerObject[target] = new HashSet<(Element, Element)>();

        activeReactionsPerObject[target].Add(reactionKey);
    }

    // ������� ����� ���� �������
    private void ProcessReaction(ElementalReaction.ReactionEffect effect, GameObject target, Vector3 position)
    {
        switch (effect.behavior)
        {
            case ElementalReaction.ReactionBehavior.SpawnEffect:
                SpawnReactionEffect(effect, target, position);
                break;
            case ElementalReaction.ReactionBehavior.ApplyStatusEffect:
                ApplyStatusEffect(effect, target);
                break;
            case ElementalReaction.ReactionBehavior.ChangeEnvironment:
                // ��� ����� ������ ����� ���� ��������
                break;
            case ElementalReaction.ReactionBehavior.ChainReaction:
                // ��� ����� ������ ����� ��������� �������
                break;
            default:
                Debug.LogWarning($"Unhandled reaction behavior: {effect.behavior}");
                break;
        }
    }

    // ��������� ������ �������
    private void SpawnReactionEffect(ElementalReaction.ReactionEffect effect, GameObject target, Vector3 position)
    {
        if (effect.effectPrefab == null) return;

        var reactionObj = Instantiate(effect.effectPrefab, position, Quaternion.identity);

        // ���������� �������� ����� ���������� �������
        if (reactionObj.TryGetComponent<AbstractReactionEffect>(out var baseEffect))
        {
            if (baseEffect is VortexDamageZone vortexEffect)
            {
                vortexEffect.Initialize(effect, effect.targetLeyer);
            }
            else
            {
                baseEffect.Initialize(effect);
            }
        }

        // ����������� ��������� �����������
        ApplyDamageInArea(position, effect);
    }

    // ������������ ����������� � ������
    private void ApplyDamageInArea(Vector3 position, ElementalReaction.ReactionEffect effect)
    {
        var colliders = Physics2D.OverlapCircleAll(position, effect.radius, effect.targetLeyer);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<ICanHit>(out var damageable))
            {
                damageable.TakeHit(effect.damage);
            }
        }
    }

    // ������������ ������-������
    private void ApplyStatusEffect(ElementalReaction.ReactionEffect effect, GameObject target)
    {
        // ����������� �����������
        if (target.TryGetComponent<ICanHit>(out var damageable))
        {
            damageable.TakeHit(effect.damage);
        }

        // ����������� ������-�����, ���� �� �
        if (effect.statusEffect != null && target.TryGetComponent<IStatusEffect>(out var statusEffectable))
        {
            //statusEffectable.Apply(effect.statusEffect); // ����� �������� BaseStatusEffect �������
        }
    }

    // �������� ������� �������
    private IEnumerator ClearActiveReaction(GameObject target, (Element, Element) reactionKey, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (activeReactionsPerObject.ContainsKey(target))
        {
            activeReactionsPerObject[target].Remove(reactionKey);

            if (activeReactionsPerObject[target].Count == 0)
                activeReactionsPerObject.Remove(target);
        }
    }

    // �������� ��� ������� ��'����
    private void OnDestroy()
    {
        activeReactionsPerObject.Clear();
    }
}