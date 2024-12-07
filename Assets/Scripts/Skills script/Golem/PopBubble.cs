using System.Collections.Generic;
using UnityEngine;

public class PopBubble : MonoBehaviour
{
    private Collider2D bubbleCollider;
    [SerializeField] LayerMask pop_layer;
    [SerializeField] LayerMask damage_layer;
    private HashSet<GameObject> _enemy = new HashSet<GameObject>();  // HashSet ��� ��������� ��������
    private void Start()
    {
        //��������� �������� ��'����
        bubbleCollider = GetComponent<Collider2D>();
        // ��������, �� �������� ������
        if (!bubbleCollider.isTrigger)
        {
            Debug.LogWarning("�������� �� � ��������");
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
        _enemy = FindUtility.FindEnemy(bubbleCollider, damage_layer);  // ������ ��� ������ � ���
        Damage.Water(new List<GameObject>(_enemy).ToArray(), 15);
        Destroy(gameObject);
    }
}
