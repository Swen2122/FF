using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WaterSpreadManager : MonoBehaviour
{
    // Синглтон для глобального доступу
    public static WaterSpreadManager Instance { get; private set; }

    [Header("Налаштування поширення води")]
    [SerializeField] private Tilemap waterTilemap; // Тайлова карта для води
    [SerializeField] private LayerMask obstacleLayer; // Шар для ідентифікації перешкод
    [SerializeField] private TileBase waterTile; // Тайл, що представляє воду

    [Header("Параметри поширення")]
    [SerializeField] private float liquidDissipationRate = 0.1f; // Швидкість випаровування води
    [SerializeField] private float minLiquidThreshold = 0.1f; // Мінімальна кількість води для видалення
    [SerializeField] private bool allowDiagonalSpread = true; // Дозволити діагональне поширення

    // Активні осередки води у симуляції
    private Dictionary<Vector3Int, WaterCell> activeWaterCells = new Dictionary<Vector3Int, WaterCell>();
    private Queue<(WaterCell cell, int remainingSteps)> spreadEvents = new Queue<(WaterCell, int)>(); // Черга подій поширення води з лічильником кроків

    private void Awake()
    {
        // Реалізація синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Автоматичне визначення тайлової карти, якщо не задано
        if (waterTilemap == null)
            waterTilemap = FindObjectOfType<Tilemap>();

        // Попередження, якщо тайл води не призначений
        if (waterTile == null)
        {
            Debug.LogWarning("Тайл води не заданий. Будь ласка, призначте його в інспекторі або через код.");
        }
    }

    private void Start()
    {
        // Запуск корутини оновлення поширення води
        StartCoroutine(WaterSpreadUpdateCycle());
    }

    private System.Collections.IEnumerator WaterSpreadUpdateCycle()
    {
        // Безперервний цикл для обробки поширення води
        while (true)
        {
            ProcessWaterSpread(Time.deltaTime); // Передача deltaTime для адаптивного оновлення
            yield return null; // Очікування до наступного кадру
        }
    }

    /// <summary>
    /// Ініціює поширення води у заданій точці світу.
    /// </summary>
    /// <param name="worldPosition">Світова позиція для початку поширення.</param>
    /// <param name="amount">Початкова кількість води.</param>
    /// <param name="spreadPressure">Початковий тиск поширення.</param>
    /// <param name="maxSteps">Максимальна кількість кроків поширення.</param>
    public void SpreadWater(Vector3 worldPosition, float amount, float spreadPressure, int maxSteps)
    {
        if (waterTilemap == null || waterTile == null)
        {
            Debug.LogError("Менеджер поширення води налаштований неправильно!");
            return;
        }

        // Перетворення світової позиції в позицію на сітці
        Vector3Int gridPosition = waterTilemap.WorldToCell(worldPosition);

        // Ініціалізація нового осередку, якщо він неактивний
        if (!activeWaterCells.ContainsKey(gridPosition))
        {
            WaterCell newWaterCell = new WaterCell(gridPosition, amount)
            {
                SpreadPressure = spreadPressure
            };

            activeWaterCells[gridPosition] = newWaterCell;
            spreadEvents.Enqueue((newWaterCell, maxSteps));

            // Встановлення тайла води на тайловій карті
            waterTilemap.SetTile(gridPosition, waterTile);
        }
    }

    /// <summary>
    /// Обробляє логіку поширення води, випаровування рідини та поширення тиску.
    /// </summary>
    /// <param name="deltaTime">Час, що минув з останнього оновлення.</param>
    private void ProcessWaterSpread(float deltaTime)
    {
        Dictionary<Vector3Int, WaterCell> newWaterCells = new();

        int processedEvents = 0;
        while (spreadEvents.Count > 0 && processedEvents < 50) // Обмеження кількості подій за кадр
        {
            var (currentCell, remainingSteps) = spreadEvents.Dequeue();

            // Зменшення кількості рідини та тиску з часом
            currentCell.LiquidAmount -= liquidDissipationRate * deltaTime;
            currentCell.SpreadPressure *= Mathf.Pow(0.8f, deltaTime);

            // Видалення осередку, якщо кількість води нижче порогу
            if (currentCell.LiquidAmount <= minLiquidThreshold || remainingSteps <= 0)
            {
                activeWaterCells.Remove(currentCell.GridPosition);
                waterTilemap.SetTile(currentCell.GridPosition, null);
                continue;
            }

            // Перевірка сусідніх осередків для поширення
            foreach (Vector3Int direction in GetSpreadDirections())
            {
                Vector3Int neighborPos = currentCell.GridPosition + direction;

                // Ігнорування позицій, які мають перешкоди
                Vector3 worldPos = waterTilemap.GetCellCenterWorld(neighborPos);
                if (Physics2D.OverlapPoint(worldPos, obstacleLayer) != null)
                    continue;

                // Розрахунок кількості води для поширення (зменшується для діагональних напрямків)
                float spreadAmount = currentCell.SpreadPressure * (direction.magnitude > 1 ? 0.5f : 1f);

                // Додавання нового осередку води або оновлення існуючого
                if (!activeWaterCells.TryGetValue(neighborPos, out WaterCell neighborCell))
                {
                    neighborCell = new WaterCell(neighborPos, spreadAmount);
                    newWaterCells[neighborPos] = neighborCell;
                    spreadEvents.Enqueue((neighborCell, remainingSteps - 1));
                    waterTilemap.SetTile(neighborPos, waterTile);
                }
                else
                {
                    neighborCell.LiquidAmount += spreadAmount;
                }
            }

            processedEvents++;
        }

        // Додавання нових осередків до активних
        foreach (var newCell in newWaterCells)
        {
            activeWaterCells[newCell.Key] = newCell.Value;
        }
    }

    /// <summary>
    /// Повертає допустимі напрямки для поширення води залежно від налаштувань.
    /// </summary>
    /// <returns>Масив напрямків.</returns>
    private IEnumerable<Vector3Int> GetSpreadDirections()
    {
        return allowDiagonalSpread
            ? new[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right,
                      new Vector3Int(1, 1, 0), new Vector3Int(-1, 1, 0),
                      new Vector3Int(1, -1, 0), new Vector3Int(-1, -1, 0) }
            : new[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
    }

    /// <summary>
    /// Представляє одиничний осередок води у симуляції.
    /// </summary>
    private struct WaterCell
    {
        public Vector3Int GridPosition { get; } // Позиція на сітці
        public float LiquidAmount { get; set; } // Поточна кількість рідини в осередку
        public float SpreadPressure { get; set; } // Поточний тиск для поширення

        public WaterCell(Vector3Int position, float amount)
        {
            GridPosition = position;
            LiquidAmount = amount;
            SpreadPressure = amount;
        }
    }
}
