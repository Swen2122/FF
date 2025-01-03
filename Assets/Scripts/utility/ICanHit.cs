using UnityEngine;

public abstract class ICanHit : MonoBehaviour
{
    /// <summary>
    /// Метод для отримання пошкоджень. Має бути реалізований у дочірніх класах.
    /// </summary>
    /// <param name="damage">Кількість пошкоджень.</param>
    /// <param name="element">Тип елемента.</param>
    public abstract void TakeHit(float damage, Element element);

    /// <summary>
    /// Перевірка, чи об'єкт вже знищено.
    /// </summary>
    /// <returns>Повертає true, якщо об'єкт знищено.</returns>
    public abstract bool IsDestroyed();

    /// <summary>
    /// Метод для повного знищення об'єкта.
    /// </summary>
    protected abstract void DestroyObject();
}