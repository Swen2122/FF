using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{   
    [Header("Sound")]
    [SerializeReference] private AudioSource audioSource;
    [SerializeReference] private AudioClip water_sound;
    [SerializeReference] private AudioClip fire_sound;
    [SerializeReference] private AudioClip earth_sound;
    [SerializeReference] private AudioClip wind_sound;
    [Header("Animation")]
    [SerializeReference] private Animator anim;
    [Header("Skills")]
    [SerializeReference] private Malee_atk weapon;  // Компонент, який обробляє нанесення шкоди
    [SerializeReference] private M2Skill m2;
    public Element_use elementUseScript;
    public Element_use elementM2;

    private HashSet<GameObject> _enemy = new HashSet<GameObject>();  // HashSet для уникнення дублікатів
    [Header("Atack")]
    public Collider2D myCollider;  // Колайдер для визначення зони атаки
    public string targetTag = "Enemy";  // Тег для визначення ворогів

    void Update()
    {   
        if (Input.GetMouseButtonDown(0))  // Ліва кнопка миші для атаки
        {
            FindEnemy();  // Знайти всіх ворогів у зоні
            Element element = elementUseScript.currentElement;
            switch (element)
            {
                case Element.Water:
                    anim.SetTrigger("water_atk");
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
        if (Input.GetMouseButtonDown(1))  // Ліва кнопка миші для атаки
        {          
            Element element = elementM2.currentElement;
            switch (element)
            {
                case Element.Water:
                    PlaySound(water_sound);
                    m2.WaterM2(10f, 10);
                    break;
                case Element.Earth:
                    PlaySound(earth_sound);
                    m2.EarthM2(30f, 30);
                    break;
                case Element.Fire:
                    
                    break;
                case Element.Wind:
                    
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

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
