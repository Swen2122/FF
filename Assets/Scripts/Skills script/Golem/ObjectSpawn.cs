using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab; // ������ ��� ��������� ��'����
    [SerializeField] private int maxObjects = 2; // ����������� ������� ��'����
    private Queue<GameObject> spawnedObjects = new Queue<GameObject>(); // ����� ��� ��������� ��'����
    public void SpawnOrMoveObject(Vector2 position)
    {
        // ���� ������� ��'���� ����� �� �����������, ��������� ����� ��'���
        if (spawnedObjects.Count < maxObjects)
        {
            GameObject newObject = Instantiate(objectPrefab, position, Quaternion.identity);
            spawnedObjects.Enqueue(newObject); // ������ ����� ��'��� � �����
        }
        else
        {
            // ���� ��'���� ��� ���, ��������� ���������� ��'��� �� ���� �������
            GameObject oldestObject = spawnedObjects.Dequeue(); // ��������� ��'��� � ������� �����
            oldestObject.transform.position = position; // ��������� ���� �� ���� �������
            spawnedObjects.Enqueue(oldestObject); // ������ ��'��� ����� � ����� �����
        }
    }
}
