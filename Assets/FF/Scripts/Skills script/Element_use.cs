using UnityEngine.UI;
using UnityEngine;

public class Element_use : MonoBehaviour
{
    [Header("UI Elements")]
    public Image icon;

    [Header("Element Configuration")]
    [SerializeField] private ElementData elementData;

    [Header("Current State")]
    public Element currentElement;

    void Start()
    {
        // ������������ ���������� �������, ���� �� �
        if (elementData.activeElements.Length > 0)
        {
            OnElementSelected(elementData.activeElements[0]);
        }
    }

    public void OnElementSelected(Element element)
    {
        // �������� �� �������� ������� � ����� ��������
        if (System.Array.IndexOf(elementData.activeElements, element) == -1)
        {
            Debug.LogWarning($"������� {element} �� � ��������!");
            return;
        }

        currentElement = element;
        Debug.Log($"������� �������: {element}");
        UpdateElementIcon();
    }

    private void UpdateElementIcon()
    {
        // ��������� ������ �� ����� ��������� ��������
        Sprite newSprite = currentElement switch
        {
            Element.Water => elementData.waterSprite,
            Element.Earth => elementData.earthSprite,
            Element.Fire => elementData.fireSprite,
            Element.Wind => elementData.windSprite,
            Element.Ice => elementData.iceSprite,
            Element.Electro => elementData.electricSprite,
            _ => null // ��� ������� Element.None ��� ��������� ��������
        };

        if (newSprite != null)
        {
            icon.sprite = newSprite;
            Debug.Log($"������ �������� ��� ��������: {currentElement}");
        }
        else
        {
            Debug.LogWarning($"�� �������� ������ ��� ��������: {currentElement}");
        }
    }

    // ����� ������ ����� ��� ����������� ��������� ������
    public void RefreshIcon()
    {
        UpdateElementIcon();
    }
}