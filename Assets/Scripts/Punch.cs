using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public Damage weapon;  // Компонент, який обробляє нанесення шкоди

    private HashSet<GameObject> _enemy = new HashSet<GameObject>();  // HashSet для уникнення дублікатів
    public Collider2D myCollider;  // Колайдер для визначення зони атаки
    public string targetTag = "Enemy";  // Тег для визначення ворогів
    public Element_use elementUseScript;
    void Update()
    {   
        if (Input.GetMouseButtonDown(0))  // Ліва кнопка миші для атаки
        {
            FindEnemy();  // Знайти всіх ворогів у зоні
            Element element = elementUseScript.currentElement;
            switch (element)
            {
                case Element.Water:
                    weapon.Water(new List<GameObject>(_enemy).ToArray(), -5);  // Викликати метод атаки і передати ворогів
                    break;
                case Element.Earth:
                    weapon.Earth(new List<GameObject>(_enemy).ToArray(), 15);
                    break;
                case Element.Fire:
                    weapon.Fire(new List<GameObject>(_enemy).ToArray(), 20);
                    break;
                case Element.Wind:
                    weapon.Wind(new List<GameObject>(_enemy).ToArray(), 10);
                    break;
            }
        }
    }

    void FindEnemy()
    {
        // Очищаємо HashSet перед новим пошуком
        _enemy.Clear();
        // Знаходимо всі колайдери в зоні колайдера об'єкта
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // Якщо колайдер належить об'єкту з тегом "Enemy"
            if (hitCollider.CompareTag(targetTag))
            {
                // Додаємо об'єкт до HashSet (унікальність забезпечується автоматично)
                _enemy.Add(hitCollider.gameObject);
            }
        }
    }
}
