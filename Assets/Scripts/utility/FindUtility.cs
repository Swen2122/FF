using System.Collections.Generic;
using UnityEngine;

public static class FindUtility
{
    public static HashSet<GameObject> FindEnemy(Collider2D meleeAttackCollider, LayerMask targetLayer)
    {
        // ���������� HashSet ��� ��������� ������
        HashSet<GameObject> enemies = new HashSet<GameObject>();

        // ��������� �� ��������� � ��� ��������� ��'����
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(meleeAttackCollider.bounds.center, meleeAttackCollider.bounds.size, 0f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // ���� �������� �������� ��'���� � ���������� ����� (���������, "Enemy")
            if (((1 << hitCollider.gameObject.layer) & targetLayer) != 0)
            {
                // ������ ��'��� �� HashSet (���������� ������������� �����������)
                enemies.Add(hitCollider.gameObject);
            }
        }

        // ��������� HashSet � ���������� ��������
        return enemies;
    }
}
