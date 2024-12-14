using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectSpawn : MonoBehaviour
{
    // Префаби для різних типів елементів
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject earthPrefab;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject windPrefab;
    [SerializeField] private int maxObjects = 2; // Максимальна кількість об'єктів

    // Окремі черги для кожного типу елементів
    private Queue<GameObject> waterObjects = new Queue<GameObject>();
    private Queue<GameObject> earthObjects = new Queue<GameObject>();
    private Queue<GameObject> fireObjects = new Queue<GameObject>();
    private Queue<GameObject> windObjects = new Queue<GameObject>();

    // Метод для створення або переміщення об'єкта на основі типу елемента
    public void SpawnOrMoveObject(Element element, Vector2 position)
    {
        // Вибір відповідного префабу та черги на основі елемента
        GameObject prefab = GetPrefabForElement(element);
        Queue<GameObject> objectQueue = GetQueueForElement(element);

        // Перевірка на null для префабу
        if (prefab == null)
        {
            Debug.LogError($"Префаб для елемента {element} не призначено!");
            return;
        }

        // Очищення черги від знищених об'єктів
        CleanDestroyedObjects(objectQueue);

        // Якщо кількість об'єктів менша за максимальну, створюємо новий об'єкт
        if (objectQueue.Count < maxObjects)
        {
            GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
            objectQueue.Enqueue(newObject);
        }
        else
        {
            // Якщо досягнуто максимальну кількість, переміщуємо найстаріший об'єкт
            GameObject oldestObject = objectQueue.Dequeue();

            // Перевірка, чи об'єкт досі існує
            if (oldestObject != null)
            {
                oldestObject.transform.position = position;
                objectQueue.Enqueue(oldestObject);
            }
            else
            {
                // Якщо найстарший об'єкт став null, створюємо новий
                GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
                objectQueue.Enqueue(newObject);
            }
        }
    }

    // Допоміжний метод для вибору префабу
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

    // Допоміжний метод для вибору черги
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

    // Метод для очищення черги від знищених об'єктів
    private void CleanDestroyedObjects(Queue<GameObject> queue)
    {
        // Видаляємо всі знищені об'єкти з черги
        var validObjects = queue.Where(obj => obj != null && obj).ToList();
        queue.Clear();
        foreach (var obj in validObjects)
        {
            queue.Enqueue(obj);
        }
    }
}