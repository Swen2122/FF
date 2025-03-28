using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementalReactionHandler : MonoBehaviour
{
    [SerializeField] private ElementalReactionDatabase reactionDatabase;

    // Словник для зберігання активних реакцій для кожного об'єкта
    private Dictionary<GameObject, HashSet<(Element, Element)>> activeReactionsPerObject
        = new Dictionary<GameObject, HashSet<(Element, Element)>>();

    // Метод для запуску реакції
    public void TriggerReaction(Element first, Element second, GameObject check,GameObject target, Vector3 position)
    {
        if(IsObjectInDictionary(check)) return;
        var reaction = reactionDatabase.GetReaction(first, second);
        if (reaction == null || target == null) return;

        var reactionKey = (first, second);

        // Перевірка, чи можна запустити реакцію
        if (!CanTriggerReaction(target, reactionKey, reaction.Effect.allowMultipleReactions))
            return;

        // Додавання реакції до активних
        AddActiveReaction(target, reactionKey);

        // Обробка ефекту реакції на цільовому об'єкті
        ProcessReaction(reaction.Effect, target, position);

        // Очищення реакції після завершення
        StartCoroutine(ClearActiveReaction(target, reactionKey, reaction.Effect.energy));
    }
    private bool IsObjectInDictionary(GameObject check)
    {
        return activeReactionsPerObject.ContainsKey(check);
    }
    // Перевірка можливості запуску реакції
    private bool CanTriggerReaction(GameObject target, (Element, Element) reactionKey, bool allowMultiple)
    {
        if (!activeReactionsPerObject.ContainsKey(target))
            return true;

        return allowMultiple || !activeReactionsPerObject[target].Contains(reactionKey);
    }

    // Додавання реакції до активних
    private void AddActiveReaction(GameObject target, (Element, Element) reactionKey)
    {
        if (!activeReactionsPerObject.ContainsKey(target))
            activeReactionsPerObject[target] = new HashSet<(Element, Element)>();

        activeReactionsPerObject[target].Add(reactionKey);
    }

    // Обробка ефекту реакції
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
                // Логіка зміни середовища
                break;
            case ElementalReaction.ReactionBehavior.ChainReaction:
                // Логіка ланцюгової реакції
                break;
            default:
                Debug.LogWarning($"Unhandled reaction behavior: {effect.behavior}");
                break;
        }
    }

    // Спавн ефекту реакції
    private void SpawnReactionEffect(ElementalReaction.ReactionEffect effect, GameObject target, Vector3 position)
    {
        if (effect.effectPrefab == null) return;

        var reactionObj = Instantiate(effect.effectPrefab, position, Quaternion.identity);

        // Ініціалізація ефекту реакції
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

        // Застосування пошкоджень у зоні
        ApplyDamageInArea(position, effect);
    }

    // Застосування пошкоджень у зоні
    private void ApplyDamageInArea(Vector3 position, ElementalReaction.ReactionEffect effect)
    {
        var colliders = Physics2D.OverlapCircleAll(position, effect.radius, effect.targetLeyer);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<ICanHit>(out var damageable))
            {
                damageable.TakeHit(effect.damage, Element.None);
            }
        }
    }

    // Застосування статус-ефекту
    private void ApplyStatusEffect(ElementalReaction.ReactionEffect effect, GameObject target)
    {
        // Застосування пошкоджень
        if (target.TryGetComponent<ICanHit>(out var damageable))
        {
            damageable.TakeHit(effect.damage, Element.Fire);
        }
    }

    // Метод для очищення активної реакції
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

    // Очищення всіх реакцій при знищенні об'єкта
    private void OnDestroy()
    {
        activeReactionsPerObject.Clear();
    }
}