using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementalReactionHandler : MonoBehaviour
{
    [SerializeField] private ElementalReactionDatabase reactionDatabase;

    // Словник для відстеження активних реакцій для кожного об'єкта
    private Dictionary<GameObject, HashSet<(Element, Element)>> activeReactionsPerObject
        = new Dictionary<GameObject, HashSet<(Element, Element)>>();

    // Тригер елементальної реакції
    public void TriggerReaction(Element first, Element second, GameObject target, Vector3 position)
    {
        var reaction = reactionDatabase.GetReaction(first, second);
        if (reaction == null || target == null) return;

        var reactionKey = (first, second);

        // Перевіряємо чи можна створити нову реакцію
        if (!CanTriggerReaction(target, reactionKey, reaction.Effect.allowMultipleReactions))
            return;

        // Додаємо реакцію до активних
        AddActiveReaction(target, reactionKey);

        // Обробляємо реакцію залежно від її типу
        ProcessReaction(reaction.Effect, target, position);

        // Запускаємо корутину для очищення реакції
        StartCoroutine(ClearActiveReaction(target, reactionKey, reaction.Effect.energy));
    }

    // Перевірка можливості створення нової реакції
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

    // Обробка різних типів реакцій
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
                // Тут можна додати логіку зміни оточення
                break;
            case ElementalReaction.ReactionBehavior.ChainReaction:
                // Тут можна додати логіку ланцюгової реакції
                break;
            default:
                Debug.LogWarning($"Unhandled reaction behavior: {effect.behavior}");
                break;
        }
    }

    // Створення ефекту реакції
    private void SpawnReactionEffect(ElementalReaction.ReactionEffect effect, GameObject target, Vector3 position)
    {
        if (effect.effectPrefab == null) return;

        var reactionObj = Instantiate(effect.effectPrefab, position, Quaternion.identity);

        // Перевіряємо наявність різних компонентів реакції
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

        // Застосовуємо початкове пошкодження
        ApplyDamageInArea(position, effect);
    }

    // Застосування пошкодження в області
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

    // Застосування статус-ефекту
    private void ApplyStatusEffect(ElementalReaction.ReactionEffect effect, GameObject target)
    {
        // Застосовуємо пошкодження
        if (target.TryGetComponent<ICanHit>(out var damageable))
        {
            damageable.TakeHit(effect.damage);
        }

        // Застосовуємо статус-ефект, якщо він є
        if (effect.statusEffect != null && target.TryGetComponent<IStatusEffect>(out var statusEffectable))
        {
            //statusEffectable.Apply(effect.statusEffect); // Тепер передаємо BaseStatusEffect напряму
        }
    }

    // Очищення активної реакції
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

    // Очищення при знищенні об'єкта
    private void OnDestroy()
    {
        activeReactionsPerObject.Clear();
    }
}