using UnityEngine;
using UnityEngine.UI;

public class RadialMenuHighlight : MonoBehaviour
{
    private Image hl_image;
    [SerializeField] private Image[] sectors; // ����� ������� ����
    [SerializeField] private GameObject high_light; // ��'��� � Image
    [Header("Colors")]
    [SerializeField] private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private Color highlightColor = new Color(1f, 1f, 1f, 0.8f);

    private int currentSectorIndex = -1;
    private float segmentFillAmount; // ���������� ��� ������ ��������
    private float degreesPerSector; // ��� ��� ������ �������

    private void Start()
    {
        // ����������, �� high_light ������
        if (high_light != null)
        {
            hl_image = high_light.GetComponent<Image>();
            if (hl_image == null)
            {
                Debug.LogError("��������� Image �� �������� �� ��'��� high_light.");
            }
        }

        // ���������� ���������� �� ��� ��� ������ �������� ���� ���
        if (sectors.Length > 0)
        {
            segmentFillAmount = 1.0f / sectors.Length; // ����� ������ ��������
            degreesPerSector = 360.0f / sectors.Length; // ��� ��� ������ �������
        }
        else
        {
            Debug.LogError("����� sectors ��������.");
        }
    }

    private void Update()
    {
        if(PauseManager.IsPaused) return;
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
        // ���� �� ������� �������
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
                hl_image.fillAmount = segmentFillAmount; // ������� �������� ��� ������ ��������

                // ��������� �� �� Z �� ���������� �������
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
