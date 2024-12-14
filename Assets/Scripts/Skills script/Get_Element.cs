using UnityEngine;

public class Get_Element : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private GameObject radialMenu; // �������� ����
    public Element_use Q, E, M1, M2; // ������ ��� ��������
    private bool isRadialMenuActive = false;
    private Element_select selectedElement;
    private int selectedSector = -1; // �������� ������

    void Start()
    {
        mainCamera = Camera.main;

        // �������� ���� �� �����
        if (radialMenu != null)
        {
            radialMenu.SetActive(false);
        }
    }

    void Update()
    {
        // ³������� ���� ��� ��������� �������� ������
        if (Input.GetMouseButtonDown(2))
        {
            Vector2 rayPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero, Mathf.Infinity, selectableLayer);

            if (hit.collider != null)
            {
                selectedElement = hit.transform.GetComponent<Element_select>();
                if (selectedElement != null)
                {
                    ShowRadialMenu(Input.mousePosition);
                }
            }
        }

        // ����������� ������ ��� ������ �������
        if (Input.GetMouseButton(2) && isRadialMenuActive)
        {
            Time.timeScale = 0.3f;
            RadialMenuHighlight radialMenuHighlight = radialMenu.GetComponent<RadialMenuHighlight>();
            selectedSector = radialMenuHighlight.GetCurrentSectorIndex(); // ���� �������
        }

        // ����������� �������� ��� ��������� ������
        if (Input.GetMouseButtonUp(2) && isRadialMenuActive)
        {
            Time.timeScale = 1f;
            AssignElementToSector();
            HideRadialMenu();
        }
    }

    private void ShowRadialMenu(Vector2 screenPosition)
    {
        if (!radialMenu.activeSelf)
        {
            radialMenu.SetActive(true);
        }

        // ������������ ������� ������� � ������
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;

        // ��������� ������� � ����� ������
        Vector3 clampedPosition = ClampToScreen(worldPosition);

        // ������ ����
        radialMenu.transform.position = clampedPosition;
        isRadialMenuActive = true;
    }

    private Vector3 ClampToScreen(Vector3 position)
    {
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(position);

        screenPoint.x = Mathf.Clamp(screenPoint.x, 50, Screen.width - 50);
        screenPoint.y = Mathf.Clamp(screenPoint.y, 50, Screen.height - 50);

        return mainCamera.ScreenToWorldPoint(screenPoint);
    }

    private void HideRadialMenu()
    {
        if (radialMenu.activeSelf)
        {
            radialMenu.SetActive(false);
        }
        isRadialMenuActive = false;
        selectedElement = null;
        selectedSector = -1;
    }

    private void AssignElementToSector()
    {
        if (selectedElement == null || selectedSector == -1) return;

        switch (selectedSector)
        {
            case 0:
                E.OnElementSelected(selectedElement.element);
                break;
            case 1:
                Q.OnElementSelected(selectedElement.element);
                break;
            case 2:
                M1.OnElementSelected(selectedElement.element);
                break;
            case 3:
                M2.OnElementSelected(selectedElement.element);
                break;
            default:
                Debug.Log("������ �� �������");
                break;
        }
    }
}
