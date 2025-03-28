using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemyAim : MonoBehaviour
{
    [SerializeField] private float lineLength = 5f;        // Довжина лінії
    [SerializeField] private Vector2 direction = Vector2.right; // Напрямок лінії (в 2D)
    [SerializeField] private Color lineColor = Color.red;  // Колір лінії

    private LineRenderer lineRenderer;

    void Start()
    {
        // Ініціалізація LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Лінія складається з 2 точок: початок та кінець

        // Налаштування кольору та ширини лінії
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Встановлення режиму для роботи в 2D
        lineRenderer.useWorldSpace = true; // Включаємо режим роботи в глобальних координатах
        UpdateLinePosition();
    }

    void Update()
    {
        // Оновлення позиції лінії, якщо її напрямок або довжина змінюється
        UpdateLinePosition();
    }

    void UpdateLinePosition()
    {
        // Початкова точка - позиція об'єкта
        Vector3 startPoint = transform.position;

        // Кінцева точка - на вказаній відстані та напрямку
        Vector3 endPoint = startPoint + (Vector3)(direction.normalized * lineLength);

        // Задаємо координати початку і кінця лінії
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}
