using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bolt : MonoBehaviour, IBulletBehavior
{
    public LayerMask targetLayerMask;   // Маска для визначення цілей
    public LayerMask pierceLayerMask;   // шари при контакті з якими буде вибухати об'єкт

    private HashSet<Rigidbody2D> connected = new HashSet<Rigidbody2D>();
    private Sequence moveSequence;

    private int bulletDamage;
    private bool isAttached = false;
    public void SetBulletProperties(int damage)
    {
        bulletDamage = damage;
    }

    public void SetSequence(Sequence sequence)
    {
        moveSequence = sequence;
    }
    private void Update()
    {
        // Перевірка на завершення анімації або зіткнення з Tilemap
        if (isAttached && moveSequence != null && !moveSequence.IsActive())
        {
            Destroy(gameObject); // Знищити об'єкт після застосування шкоди
        }
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
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Якщо об'єкт зіткнувся з Tilemap
        if (((1 << collision.gameObject.layer) & pierceLayerMask) != 0)
        {
            moveSequence.Kill();
            isAttached = true;
        }
    }
}
