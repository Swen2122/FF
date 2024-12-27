using UnityEngine;
using UnityEngine.UI;

public class RadialMenuHighlight : MonoBehaviour
{
    private Image hl_image;
    [SerializeField] private Image[] sectors; // Масив секторів меню
    [SerializeField] private GameObject high_light; // Об'єкт з Image
    [Header("Colors")]
    [SerializeField] private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private Color highlightColor = new Color(1f, 1f, 1f, 0.8f);

    private int currentSectorIndex = -1;
    private float segmentFillAmount; // Заповнення для одного сегмента
    private float degreesPerSector; // Кут для одного сектора

    private void Start()
    {
        // Перевіряємо, чи high_light задано
        if (high_light != null)
        {
            hl_image = high_light.GetComponent<Image>();
            if (hl_image == null)
            {
                Debug.LogError("Компонент Image не знайдено на об'єкті high_light.");
            }
        }

        // Обчислюємо заповнення та кут для одного сегмента один раз
        if (sectors.Length > 0)
        {
            segmentFillAmount = 1.0f / sectors.Length; // Розмір одного сегмента
            degreesPerSector = 360.0f / sectors.Length; // Кут для одного сектора
        }
        else
        {
            Debug.LogError("Масив sectors порожній.");
        }
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        Vector2 mousePosition = Input.mousePosition;
        Vector2 menuCenter = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        Vector2 direction = mousePosition - menuCenter;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        int newSectorIndex = DetermineSectorIndex(angle);

        if (newSectorIndex != currentSectorIndex)
        {
            UpdateSectorHighlight(newSectorIndex);
        }
    }

    private int DetermineSectorIndex(float angle)
    {
        // Поділ на кількість секторів
        return Mathf.FloorToInt(angle / degreesPerSector);
    }

    private void UpdateSectorHighlight(int newSectorIndex)
    {
        ResetSectorColors();

        if (newSectorIndex >= 0 && newSectorIndex < sectors.Length)
        {
            sectors[newSectorIndex].color = highlightColor;

            if (hl_image != null)
            {
                hl_image.fillAmount = segmentFillAmount; // Постійне значення для одного сегмента

                // Обертання по осі Z до відповідного сектора
                float rotationAngle = newSectorIndex * degreesPerSector;
                high_light.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
            }

            currentSectorIndex = newSectorIndex;
        }
    }

    private void ResetSectorColors()
    {
        foreach (var sector in sectors)
        {
            sector.color = defaultColor;
        }
    }

    public int GetCurrentSectorIndex()
    {
        return currentSectorIndex;
    }
}
