using UnityEngine;
using System.Collections.Generic;
public abstract class ICanHit : MonoBehaviour
{
    protected Dictionary<Element, float> externalEnergies = new Dictionary<Element, float>();
    protected Dictionary<Element, float> internalEnergies = new Dictionary<Element, float>();
    /// <summary>
    /// Метод для отримання пошкоджень. Він буде реалізований у похідних класах.
    /// </summary>
    /// <param name="damage">Кількість пошкоджень.</param>
    /// <param name="element">Тип елемента.</param>
    public virtual void TakeHit(float damage, Element element)
    {
        Debug.Log("Taking hit");
        AddEnergy(damage, element);
        CheckReactions();
    }
    public virtual void TakeEnergy(float energy, Element element)
    {
        AddEnergy(energy, element);

    }
    public virtual void AddEnergy(float energy, Element element)
    {
        if (externalEnergies.ContainsKey(element))
        {
            externalEnergies[element] += energy;
        }
        else
        {
            externalEnergies.Add(element, energy);
        }
    }
    public virtual void AddInternalEnergy(float energy, Element element)
    {
        if (internalEnergies.ContainsKey(element))
        {
            internalEnergies[element] += energy;
        }
        else
        {
            internalEnergies.Add(element, energy);
        }
        foreach (var pair in internalEnergies)
        {
            Debug.Log($"Element: {pair.Key}, Energy: {pair.Value}");
        }
    }
    public virtual float GetEnergy(Element element)
    {
        if (internalEnergies.ContainsKey(element))
        {
            return internalEnergies[element];
        }
        return 0f;
    }

    public virtual void CheckReactions()
    {
        Debug.Log("Checking reactions");

        List<Element> elements = new List<Element>(externalEnergies.Keys);

        if (elements.Count < 2) return; // Якщо немає пар – вихід

        // Перебираємо всі пари, уникаючи дублювання (A, B) == (B, A)
        for (int i = 0; i < elements.Count; i++)
        {
            Element elementA = elements[i];

            for (int j = i + 1; j < elements.Count; j++) // j = i+1 -> уникаємо повторів
            {
            Element elementB = elements[j];

            Debug.Log($"Checking reaction between {elementA} and {elementB}");
            StatusEffectManager.AddStatusEffect(elementA, elementB);
            }
        }
    }


    /// <summary>
    /// Перевіряє, чи об'єкт вже знищений.
    /// </summary>
    /// <returns>Повертає true, якщо об'єкт знищений.</returns>
    public abstract bool IsDestroyed();

    /// <summary>
    /// Метод для знищення об'єкта.
    /// </summary>
    protected abstract void DestroyObject();
}