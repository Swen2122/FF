using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpringGrab : MonoBehaviour, IBulletBehavior
{
    public LayerMask targetLayerMask;   // Маска для визначення цілей
    public LayerMask boomLayerMask;   // шари при контакті з якими буде вибухати об'єкт
    public float damageRadius = 2f;      // Радіус завдання шкоди

    private HashSet<Rigidbody2D> connected = new HashSet<Rigidbody2D>();
    private Sequence moveSequence;

    private int bulletDamage;
    private bool isAttached = false; // Перевірка, чи прикріплений об'єкт
    public void SetBulletProperties(int damage)
    {
        bulletDamage = damage;
    }

    public void SetSequence(Sequence sequence)
    {
        moveSequence = sequence;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D otherRigidbody = other.attachedRigidbody;
        if (((1 << other.gameObject.layer) & targetLayerMask) != 0 && otherRigidbody != null && !connected.Contains(otherRigidbody))
        {
            // Створюємо FixedJoint2D
            FixedJoint2D fixedJoint = gameObject.AddComponent<FixedJoint2D>();
            fixedJoint.breakForce = Mathf.Infinity; // Без розриву
            fixedJoint.breakTorque = Mathf.Infinity; // Без розриву
            // З'єднуємо з іншим об'єктом
            fixedJoint.connectedBody = otherRigidbody;
            fixedJoint.autoConfigureConnectedAnchor = false;

            // Додаємо в список підключених тіл
            connected.Add(otherRigidbody);
            isAttached = true; // Встановлюємо, що об'єкт прикріплений
        }
    }

    private void Update()
    {
        // Перевірка на завершення анімації або зіткнення з Tilemap
        if (isAttached && moveSequence != null && !moveSequence.IsActive())
        {
            ApplyDamage();
            Destroy(gameObject); // Знищити об'єкт після застосування шкоди
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Якщо об'єкт зіткнувся з Tilemap
        if (((1 << collision.gameObject.layer) & boomLayerMask) != 0)
        {
            moveSequence.Kill();
            ApplyDamage();
            Destroy(gameObject); // Знищити об'єкт після застосування шкоди
        }
    }
    private void ApplyDamage()
    {
        // Завдати шкоди всім ворогам у зоні
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, damageRadius, targetLayerMask);

        foreach (var enemy in enemiesHit)
        {
            // Перевіряємо, чи об'єкт відповідає шару
            if (LaymaskUtility.InIsLMask(enemy.gameObject, targetLayerMask))
            {
                // Отримуємо компонент, який реалізує ICanHit
                if (enemy.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    hitTarget.TakeHit(bulletDamage); // Завдаємо шкоди через інтерфейс
                }
            }
        }
    }

}
