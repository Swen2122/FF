using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Skill_System : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip water_sound;
    [SerializeField] private AudioClip fire_sound;
    [SerializeField] private AudioClip earth_sound;
    [SerializeField] private AudioClip wind_sound;
    [SerializeField] private AudioMixer audioMixer;
    private float currentPitch = 0.75f;
    [Header("Animation")]
    [SerializeField] private Animator anim;
    [Header("Skills")]
    [SerializeField] private ParabolicSkill E;
    [SerializeField] private M2Skill m2;
    [SerializeField] private ObjectSpawn Q;
    [SerializeField] private Element_use elementM1;
    [SerializeField] private Element_use elementM2;
    [SerializeField] private Element_use elementE;
    [SerializeField] private Element_use elementQ;
    [Header("Atack")]
    private HashSet<GameObject> _enemy = new HashSet<GameObject>();  // HashSet для уникнення дублікатів
    public Collider2D MeleeAttack;  // Колайдер для визначення зони атаки
    [Header("Шар на який задівають атаки")]
    public LayerMask targetLayer;
    [Header("Utility")]
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {   
        if (Input.GetMouseButtonDown(0))  // Ліва кнопка миші для атаки
        {
            _enemy = FindUtility.FindEnemy(MeleeAttack, targetLayer);  // Знайти всіх ворогів у зоні
            Element element = elementM1.currentElement;
            switch (element)
            {
                case Element.Water:
                    anim.SetTrigger("water_atk");
                    Damage.ApplyDamage(new List<GameObject>(_enemy).ToArray(), -5, element);  // Викликати метод атаки і передати ворогів
                    HitStop.TriggerStop(0.05f, 0.0f);
                    foreach (GameObject enemy in _enemy)
                    {
                        // Отримуємо Rigidbody2D ворога для застосування фізики
                        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

                        // Якщо ворог має Rigidbody2D
                        if (enemyRb != null)
                        {
                            // Використовуємо метод Push для відштовхування ворога
                            PushUtility.Push(enemyRb, transform.position, -3f);
                        }
                    }
                    break;
                case Element.Earth:
                    HitStop.TriggerStop(0.05f, 0.0f);
                    Damage.ApplyDamage(new List<GameObject>(_enemy).ToArray(), 15, element);
                    foreach (GameObject enemy in _enemy)
                    {
                        // Отримуємо Rigidbody2D ворога для застосування фізики
                        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

                        // Якщо ворог має Rigidbody2D
                        if (enemyRb != null)
                        {
                            // Використовуємо метод Push для відштовхування ворога
                            PushUtility.Push(enemyRb, transform.position, 10f);
                        }
                    }
                    break;
                case Element.Fire:
                    anim.SetTrigger("fire_atk");
                    HitStop.TriggerStop(0.05f, 0.0f);
                    Damage.ApplyDamage(new List<GameObject>(_enemy).ToArray(), 20, element);
                    foreach (GameObject enemy in _enemy)
                    {
                        // Отримуємо Rigidbody2D ворога для застосування фізики
                        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

                        // Якщо ворог має Rigidbody2D
                        if (enemyRb != null)
                        {
                            // Використовуємо метод Push для відштовхування ворога
                            PushUtility.Push(enemyRb, transform.position, 15f);
                        }
                    }
                    break;
                case Element.Wind:
                    HitStop.TriggerStop(0.05f, 0.0f);
                    Damage.ApplyDamage(new List<GameObject>(_enemy).ToArray(), 10, element);
                    foreach (GameObject enemy in _enemy)
                    {
                        // Отримуємо Rigidbody2D ворога для застосування фізики
                        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

                        // Якщо ворог має Rigidbody2D
                        if (enemyRb != null)
                        {
                            // Використовуємо метод Push для відштовхування ворога
                            PushUtility.Push(enemyRb, transform.position, 5f);
                        }
                    }
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
                    PitchChanger.ChangePitch(audioMixer, ref currentPitch, 0.05f);
                    m2.WaterM2();
                    break;
                case Element.Earth:
                    PlaySound(earth_sound);
                    m2.EarthM2(30f, 30);
                    break;
                case Element.Fire:
                    
                    break;
                case Element.Wind:
                    E.Activate(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Null checks
            if (mainCamera == null)
            {
                Debug.LogError("Main camera is not assigned!");
                return;
            }

            if (Q == null)
            {
                Debug.LogError("Q object is not assigned!");
                return;
            }

            if (elementQ?.currentElement == null)
            {
                Debug.LogError("Current element is null!");
                return;
            }
            Vector2 spawnPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);// Отримуємо позицію курсора в світових координатах
            Q.SpawnOrMoveObject(elementQ.currentElement, spawnPosition);
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
