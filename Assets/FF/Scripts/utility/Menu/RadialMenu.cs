using UnityEngine;
using UnityEngine.UI;

public enum SkillSlot
{
    Q,
    E,
    M1,
    M2,
    Dash
}

public class RadialMenu : MonoBehaviour
{
    [SerializeField] private GameObject radialMenuPrefab;
    [SerializeField] private Element_use qSkill;
    [SerializeField] private Element_use eSkill;
    [SerializeField] private Element_use m1Skill;
    [SerializeField] private Element_use m2Skill;
    [SerializeField] private Element_use dashSkill;

    private GameObject currentRadialMenu;
    private RectTransform radialMenuRectTransform;
    private Vector2 startMousePosition;
    private Element currentSelectedElement;

    // ����� ��� ������������ ��������� ��������
    public void SetCurrentElement(Element element)
    {
        currentSelectedElement = element;
    }

    void Update()
    {
        // ³������� ���� �� Middle Mouse Button
        if (Input.GetMouseButtonDown(2))
        {
            OpenRadialMenu();
        }

        // �������� ���������� ���� ��� ���������� ������ ����
        if (Input.GetMouseButtonUp(2) && currentRadialMenu != null)
        {
            AssignElementToSkill();
            CloseRadialMenu();
        }
    }

    void OpenRadialMenu()
    {
        // ���� ���� �� �� ��������, ��������� ����
        if (currentRadialMenu == null)
        {
            currentRadialMenu = Instantiate(radialMenuPrefab, transform);
        }

        // �������� ����
        currentRadialMenu.SetActive(true);

        radialMenuRectTransform = currentRadialMenu.GetComponent<RectTransform>();

        // ������������ ������� ���� � ����� ������
        radialMenuRectTransform.position = Input.mousePosition;

        // �������� initial ������� ����
        startMousePosition = Input.mousePosition;
    }

    void AssignElementToSkill()
    {
        // ���������� ���� �� ������ ����
        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 direction = currentMousePosition - startMousePosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ����������� ���� (0-360 �������)
        if (angle < 0) angle += 360;

        // ���� ����� ����� ������� �� ����
        SkillSlot selectedSkill = DetermineSkillFromAngle(angle);

        // ����������� �������� � ������� ���� �����
        AssignElementToSelectedSkill(selectedSkill);
    }

    SkillSlot DetermineSkillFromAngle(float angle)
    {
        // ��������� ���� �� 5 ������ �� 72 �������
        if (angle >= 0 && angle < 72) return SkillSlot.Q;
        if (angle >= 72 && angle < 144) return SkillSlot.E;
        if (angle >= 144 && angle < 216) return SkillSlot.M1;
        if (angle >= 216 && angle < 288) return SkillSlot.M2;
        return SkillSlot.Dash;
    }

    void AssignElementToSelectedSkill(SkillSlot skill)
    {
        // ����������� �������� � ������� ���� �����
        switch (skill)
        {
            case SkillSlot.Q:
                qSkill.OnElementSelected(currentSelectedElement);
                break;
            case SkillSlot.E:
                eSkill.OnElementSelected(currentSelectedElement);
                break;
            case SkillSlot.M1:
                m1Skill.OnElementSelected(currentSelectedElement);
                break;
            case SkillSlot.M2:
                m2Skill.OnElementSelected(currentSelectedElement);
                break;
            case SkillSlot.Dash:
                dashSkill.OnElementSelected(currentSelectedElement);
                break;
        }
    }

    void CloseRadialMenu()
    {
        // ����������� ���������� ����
        if (currentRadialMenu != null)
        {
            currentRadialMenu.SetActive(false);
        }
    }
}