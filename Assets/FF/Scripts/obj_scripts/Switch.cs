using UnityEngine;
using DG.Tweening;
public class Switch : MonoBehaviour, IInteractible
{
    [SerializeField] private GateManipulator gate; // ���������� ��� ����
    [SerializeField] private Transform switchHandle; // ��������� ��'����, ���� ������� ��������
    [SerializeField] private float rotationDuration = 0.5f; // ��������� ���������
    [SerializeField] private Ease rotationEase = Ease.OutQuad; // ��� easing ��� ���������
    private bool isActive = false;
    public void Use()
    {
        if (!isActive)
        {
            On();
            isActive = true;
        }
        else 
        {
            Off(); 
            isActive = false; 
        }
    }

    public void On()
    {
        gate.OpenGate();
        RotateHandle(-35); // ������ ��������� �� 35 �������
    }

    public void Off()
    {
        gate.CloseGate();
        RotateHandle(35); // ���������� �� ����������� ���������
    }
    private void RotateHandle(float targetAngle)
    {
        // ������ �������� switchHandle �� �������� ����
        switchHandle.DOLocalRotate(new Vector3(0, 0, targetAngle), rotationDuration)
            .SetEase(rotationEase); // ������������� ����� �������
    }
}
