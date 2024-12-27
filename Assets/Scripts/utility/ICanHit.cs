using UnityEngine;

public interface ICanHit
{
    /// <summary>
    /// Метод для отримання пошкоджень.
    /// </summary>
    /// <param name="DMG">Кількість пошкоджень, які отримує об'єкт.</param>
    void TakeHit(float damage);
}