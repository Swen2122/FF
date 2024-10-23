using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Get_Element : MonoBehaviour
{
    public Camera mainCamera; // ����� �� ������
    [SerializeReference] private LayerMask selectableLayer;
    public Element_use Q;
    public Element_use E;
    public Element_use M1;
    public Element_use M2;
    public Element_use dash;

    void Update()
    {
        // ����������, �� ���� ��������� ��� ������ ����
        if (Input.GetMouseButtonDown(2))
        {
            // ��������� ������ � ������ � �������� �� ������� ������� ����
            Vector2 rayPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero);

            // ���� ������ ����� � ��'���
            if (hit.collider != null)
            {
                // �������� ��������� Element_select � ��'����
                Element_select elementSelect = hit.transform.GetComponent<Element_select>();
                //��� ��������� ������ �����, ��� ���� ����������
                if ((elementSelect != null) && Input.GetButton("M1"))
                {
                    M1.OnElementSelected(elementSelect.element);
                    Debug.Log("��� � Element_selector: " + elementSelect.element);
                }
                if ((elementSelect != null) && Input.GetButton("M2"))
                {
                    M2.OnElementSelected(elementSelect.element);
                    Debug.Log("��� � Element_selector: " + elementSelect.element);
                }
                
                if ((elementSelect != null) && Input.GetButton("Q"))
                {
                    Q.OnElementSelected(elementSelect.element);
                    Debug.Log("��� � Element_selector: " + elementSelect.element);
                }

                if ((elementSelect != null) && Input.GetButton("E"))
                {
                    E.OnElementSelected(elementSelect.element);
                    Debug.Log("��� � Element_selector: " + elementSelect.element);
                }

                if ((elementSelect != null) && Input.GetButton("1"))
                {
                    dash.OnElementSelected(elementSelect.element);
                    Debug.Log("��� � Element_selector: " + elementSelect.element);
                }
            }
        }
    }
}

