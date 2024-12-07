using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab; // Префаб для створення об'єктів
    [SerializeField] private int maxObjects = 2; // Максимальна кількість об'єктів
    private Queue<GameObject> spawnedObjects = new Queue<GameObject>(); // Черга для зберігання об'єктів
    public void SpawnOrMoveObject(Vector2 position)
    {
        // Якщо кількість об'єктів менша за максимальну, створюємо новий об'єкт
        if (spawnedObjects.Count < maxObjects)
        {
            GameObject newObject = Instantiate(objectPrefab, position, Quaternion.identity);
            spawnedObjects.Enqueue(newObject); // Додаємо новий об'єкт у чергу
        }
        else
        {
            // Якщо об'єктів вже два, переміщуємо найстаріший об'єкт на нову позицію
            GameObject oldestObject = spawnedObjects.Dequeue(); // Видаляємо об'єкт з початку черги
            oldestObject.transform.position = position; // Переміщаємо його на нову позицію
            spawnedObjects.Enqueue(oldestObject); // Додаємо об'єкт знову в кінець черги
        }
    }
}
