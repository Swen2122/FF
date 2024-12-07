using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WaterGrab : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;      // Лейєр для цілей
    [SerializeField] private LayerMask obstacleLayer;    // Лейєр для перешкод
    [SerializeField] private float maxDistance = 10f;    // Максимальна відстань захоплення
    [SerializeField] private float pullForce = 5f;       // Сила притягування
    [SerializeField] private GameObject bubble;          // Об'єкт, який спавниться на цілі

    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    private Transform selectedTarget;
    private Rigidbody2D targetRigidbody;
    private GameObject spawnedBubble; // Збережене посилання на створену бульбашку

    void Start()
    {
        // Отримуємо основну камеру
        mainCamera = Camera.main;

        // Ініціалізуємо LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        // Ініціалізуємо SpringJoint2D (використовується для притягання)
        springJoint = gameObject.AddComponent<SpringJoint2D>();
        springJoint.enabled = false;
        springJoint.autoConfigureDistance = false;
        springJoint.frequency = 1f;   // Жорсткість "пружини"
        springJoint.dampingRatio = 0.5f; // Гасіння
    }

    void Update()
    {
        // Якщо ціль захоплена
        if (selectedTarget != null)
        {
            DrawLineToTarget();

            // ПКМ притягує ціль ближче
            if (Input.GetMouseButtonDown(0))
            {
                PullTarget();
            }

            // Клавіша "R" для скидання захоплення
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReleaseTarget();
            }
        }
    }

    public void TryCaptureTarget()
    {
        // Отримуємо позицію курсора у світових координатах
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Виконуємо Raycast для пошуку цілі
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, maxDistance, targetLayer);

        if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & targetLayer) != 0)
        {
            // Перевіряємо наявність перешкод між гравцем і ціллю
            if (!Physics2D.Linecast(transform.position, hit.collider.transform.position, obstacleLayer))
            {
                // Захоплюємо ціль
                selectedTarget = hit.collider.transform;
                targetRigidbody = selectedTarget.GetComponent<Rigidbody2D>();

                if (targetRigidbody != null)
                {
                    Bubble();
                    // Підключаємо SpringJoint2D до цілі
                    springJoint.enabled = true;
                    springJoint.connectedBody = targetRigidbody;
                    springJoint.distance = Vector2.Distance(transform.position, selectedTarget.position);

                    // Увімкнення лінії
                    lineRenderer.enabled = true;
                }
            }
        }
    }

    private void Bubble()
    {
        if (selectedTarget != null && bubble != null)
        {
            // Якщо бульбашка вже існує, не створюємо нову
            if (spawnedBubble != null)
            {
                Debug.LogWarning("Bubble already exists on target.");
                return;
            }

            // Спавнимо бульбашку на позиції цілі та отримуємо посилання на її колайдер
            spawnedBubble = Instantiate(bubble, selectedTarget.position, Quaternion.identity);
            // Встановлюємо бульбашку як дочірній об'єкт цілі
            spawnedBubble.transform.SetParent(selectedTarget);
            Debug.Log("Bubble spawned on target: " + selectedTarget.name);
        }
    }

    private void DrawLineToTarget()
    {
        if (selectedTarget != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position); // Початок у гравця
            lineRenderer.SetPosition(1, selectedTarget.position); // Кінець у цілі
        }
    }

    private void PullTarget()
    {
        if (selectedTarget != null)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pushDirection = (mousePosition - (Vector2)selectedTarget.position).normalized;
            float distance = Vector2.Distance(transform.position, selectedTarget.position);

            if (targetRigidbody != null && distance <= maxDistance)
            {
                float forceMultiplier = Mathf.Clamp01((maxDistance - distance) / maxDistance);
                targetRigidbody.AddForce(pushDirection * pullForce * forceMultiplier, ForceMode2D.Impulse);
            }

            ReleaseTarget();
        }
    }

    private void ReleaseTarget()
    {
       /* // Видаляємо бульбашку, якщо вона існує
        if (spawnedBubble != null)
        {
            Destroy(spawnedBubble);
            spawnedBubble = null;
            Debug.Log("Bubble removed from target.");
        }
       */

        // Скидаємо вибір цілі
        selectedTarget = null;
        targetRigidbody = null;

        // Вимикаємо SpringJoint2D і лінію
        springJoint.enabled = false;
        lineRenderer.enabled = false;
    }
}