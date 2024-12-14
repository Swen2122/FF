using UnityEngine;
using UnityEngine.UI;

public class RadialMenuHighlight : MonoBehaviour
{
    [SerializeField] private Image[] sectors;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject high_light;
    [Header("Colors")]
    [SerializeField] private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private Color highlightColor = new Color(1f, 1f, 1f, 0.8f);

    private int currentSectorIndex = -1;

    void Update()
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

    int DetermineSectorIndex(float angle)
    {
        // Поділ на 4 сектори по 90 градусів
        if (angle >= 0 && angle < 90) return 0;      // Q
        if (angle >= 90 && angle < 180) return 1;    // E
        if (angle >= 180 && angle < 270) return 2;   // M1
        return 3;                                    // M2
    }

    void UpdateSectorHighlight(int newSectorIndex)
    {
        ResetSectorColors();

        if (newSectorIndex >= 0 && newSectorIndex < sectors.Length)
        {
            sectors[newSectorIndex].color = highlightColor;
            if (high_light != null)
            {
                high_light.transform.rotation = Quaternion.Euler(0f, 0f, newSectorIndex*90);
            }
            currentSectorIndex = newSectorIndex;
        }
    }

    void ResetSectorColors()
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
