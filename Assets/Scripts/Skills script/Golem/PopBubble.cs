using System.Collections.Generic;
using UnityEngine;

public class PopBubble : MonoBehaviour
{
    private Collider2D bubbleCollider;
    [SerializeField] LayerMask pop_layer;
    [SerializeField] LayerMask damage_layer;
    private HashSet<GameObject> _enemy = new HashSet<GameObject>();  // HashSet для уникнення дублікатів
    private void Start()
    {
        //знаходимо колайдер об'єкта
        bubbleCollider = GetComponent<Collider2D>();
        // перевірка, чи колайдер тригер
        if (!bubbleCollider.isTrigger)
        {
            Debug.LogWarning("Колайдер не є тригером");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(LaymaskUtility.InIsLMask(collision.gameObject, pop_layer))
        {
            OnLayerContact();
        }
    }
    private void OnLayerContact()
    {
        _enemy = FindUtility.FindEnemy(bubbleCollider, damage_layer);  // Знайти всіх ворогів у зоні
        Damage.Water(new List<GameObject>(_enemy).ToArray(), 15);
        Destroy(gameObject);
    }
}
