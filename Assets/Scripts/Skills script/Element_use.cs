using UnityEngine.UI;
using UnityEngine;

public class Element_use : MonoBehaviour
{
    [Header("UI Elements")]
    public Image icon;

    [Header("Element Configuration")]
    [SerializeField] private ElementData elementData;

    [Header("Current State")]
    public Element currentElement;

    void Start()
    {
        // Встановлюємо початковий елемент, якщо він є
        if (elementData.activeElements.Length > 0)
        {
            OnElementSelected(elementData.activeElements[0]);
        }
    }

    public void OnElementSelected(Element element)
    {
        // Перевірка чи вибраний елемент є серед активних
        if (System.Array.IndexOf(elementData.activeElements, element) == -1)
        {
            Debug.LogWarning($"Елемент {element} не є активним!");
            return;
        }

        currentElement = element;
        Debug.Log($"Вибрано елемент: {element}");
        UpdateElementIcon();
    }

    private void UpdateElementIcon()
    {
        // Оновлюємо іконку на основі поточного елементу
        Sprite newSprite = currentElement switch
        {
            Element.Water => elementData.waterSprite,
            Element.Earth => elementData.earthSprite,
            Element.Fire => elementData.fireSprite,
            Element.Wind => elementData.windSprite,
            Element.Ice => elementData.iceSprite,
            Element.Electro => elementData.electricSprite,
            _ => null // Для випадку Element.None або невідомого елементу
        };

        if (newSprite != null)
        {
            icon.sprite = newSprite;
            Debug.Log($"Іконку оновлено для елементу: {currentElement}");
        }
        else
        {
            Debug.LogWarning($"Не знайдено спрайт для елементу: {currentElement}");
        }
    }

    // Можна додати метод для примусового оновлення іконки
    public void RefreshIcon()
    {
        UpdateElementIcon();
    }
}