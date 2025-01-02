using System.Collections.Generic;
using UnityEngine;

public class trap_script : MonoBehaviour
{
    private HashSet<GameObject> _enemy = new HashSet<GameObject>(); // HashSet для унікальних ворогів
    public Collider2D myCollider; // Колайдер для визначення зони атаки
    [Header("Шар на який задівають атаки")]
    public LayerMask targetLayer;

    [Header("Animation")]
    [SerializeReference] private Animator anim;
    private bool isActive = false; // Активність пастки

    [Header("Sound")]
    [SerializeReference] private AudioSource audioSource;
    [SerializeReference] private AudioClip beep_audio;
    [SerializeReference] private AudioClip explosion_audio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidTarget(other))
        {
            if (_enemy.Add(other.gameObject)) // Додаємо об'єкт до списку ворогів
            {
                StartTrap(); // Запускаємо пастку
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsValidTarget(other) && !isActive)
        {
            StartTrap(); // Перезапускаємо, якщо пастка була неактивною
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsValidTarget(other))
        {
            _enemy.Remove(other.gameObject); // Видаляємо об'єкт із HashSet
            if (_enemy.Count == 0)
            {
                StopTrap(); // Зупиняємо пастку, коли ворогів немає
            }
        }
    }

    private void StartTrap()
    {
        anim.SetBool("isActive", true); // Включаємо анімацію
        isActive = true; // Пастка активна
        beep(); // Відтворюємо звук попередження
    }

    private void StopTrap()
    {
        anim.SetBool("isActive", false); // Зупиняємо анімацію
        isActive = false; // Пастка неактивна
    }

    public void boom()
    {
        FindEnemy(); // Оновлюємо список ворогів
        Damage.ApplyDamage(new List<GameObject>(_enemy).ToArray(), 40, Element.None); // Атакуємо всіх у зоні
        explosion(); // Відтворюємо звук вибуху
    }

    private void beep()
    {
        if (audioSource != null && beep_audio != null)
        {
            audioSource.PlayOneShot(beep_audio);
        }
    }

    private void explosion()
    {
        if (audioSource != null && explosion_audio != null)
        {
            audioSource.PlayOneShot(explosion_audio);
        }
    }

    private void FindEnemy()
    {
        _enemy.Clear(); // Очищаємо HashSet
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (IsValidTarget(hitCollider))
            {
                _enemy.Add(hitCollider.gameObject);
            }
        }
    }

    // Перевіряє, чи об'єкт належить до потрібного шару та не є тригером
    private bool IsValidTarget(Collider2D collider)
    {
        return ((1 << collider.gameObject.layer) & targetLayer) != 0 && !collider.isTrigger;
    }
}
