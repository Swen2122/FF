using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public Damage weapon;  // ���������, ���� �������� ��������� �����

    private HashSet<GameObject> _enemy = new HashSet<GameObject>();  // HashSet ��� ��������� ��������
    public Collider2D myCollider;  // �������� ��� ���������� ���� �����
    public string targetTag = "Enemy";  // ��� ��� ���������� ������
    public Element_use elementUseScript;
    void Update()
    {   
        if (Input.GetMouseButtonDown(0))  // ˳�� ������ ���� ��� �����
        {
            FindEnemy();  // ������ ��� ������ � ���
            Element element = elementUseScript.currentElement;
            switch (element)
            {
                case Element.Water:
                    weapon.Water(new List<GameObject>(_enemy).ToArray(), -5);  // ��������� ����� ����� � �������� ������
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
        // ������� HashSet ����� ����� �������
        _enemy.Clear();
        // ��������� �� ��������� � ��� ��������� ��'����
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // ���� �������� �������� ��'���� � ����� "Enemy"
            if (hitCollider.CompareTag(targetTag))
            {
                // ������ ��'��� �� HashSet (���������� ������������� �����������)
                _enemy.Add(hitCollider.gameObject);
            }
        }
    }
}
