using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectSpawn : MonoBehaviour
{
    // ������� ��� ����� ���� ��������
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject earthPrefab;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject windPrefab;
    [SerializeField] private int maxObjects = 2; // ����������� ������� ��'����

    // ����� ����� ��� ������� ���� ��������
    private Queue<GameObject> waterObjects = new Queue<GameObject>();
    private Queue<GameObject> earthObjects = new Queue<GameObject>();
    private Queue<GameObject> fireObjects = new Queue<GameObject>();
    private Queue<GameObject> windObjects = new Queue<GameObject>();

    // ����� ��� ��������� ��� ���������� ��'���� �� ����� ���� ��������
    public void SpawnOrMoveObject(Element element, Vector2 position)
    {
        // ���� ���������� ������� �� ����� �� ����� ��������
        GameObject prefab = GetPrefabForElement(element);
        Queue<GameObject> objectQueue = GetQueueForElement(element);

        // �������� �� null ��� �������
        if (prefab == null)
        {
            Debug.LogError($"������ ��� �������� {element} �� ����������!");
            return;
        }

        // �������� ����� �� �������� ��'����
        CleanDestroyedObjects(objectQueue);

        // ���� ������� ��'���� ����� �� �����������, ��������� ����� ��'���
        if (objectQueue.Count < maxObjects)
        {
            GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
            objectQueue.Enqueue(newObject);
        }
        else
        {
            // ���� ��������� ����������� �������, ��������� ���������� ��'���
            GameObject oldestObject = objectQueue.Dequeue();

            // ��������, �� ��'��� ��� ����
            if (oldestObject != null)
            {
                oldestObject.transform.position = position;
                objectQueue.Enqueue(oldestObject);
            }
            else
            {
                // ���� ���������� ��'��� ���� null, ��������� �����
                GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
                objectQueue.Enqueue(newObject);
            }
        }
    }

    // ��������� ����� ��� ������ �������
    private GameObject GetPrefabForElement(Element element)
    {
        return element switch
        {
            Element.Water => waterPrefab,
            Element.Earth => earthPrefab,
            Element.Fire => firePrefab,
            Element.Wind => windPrefab,
            _ => null
        };
    }

    // ��������� ����� ��� ������ �����
    private Queue<GameObject> GetQueueForElement(Element element)
    {
        return element switch
        {
            Element.Water => waterObjects,
            Element.Earth => earthObjects,
            Element.Fire => fireObjects,
            Element.Wind => windObjects,
            _ => null
        };
    }

    // ����� ��� �������� ����� �� �������� ��'����
    private void CleanDestroyedObjects(Queue<GameObject> queue)
    {
        // ��������� �� ������ ��'���� � �����
        var validObjects = queue.Where(obj => obj != null && obj).ToList();
        queue.Clear();
        foreach (var obj in validObjects)
        {
            queue.Enqueue(obj);
        }
    }
}