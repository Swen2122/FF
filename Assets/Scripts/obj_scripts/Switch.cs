using UnityEngine;
using DG.Tweening;
public class Switch : MonoBehaviour, IInteractible
{
    [SerializeField] private GateManipulator gate; // Маніпулятор для воріт
    [SerializeField] private Transform switchHandle; // Трансформ об'єкта, який потрібно обертати
    [SerializeField] private float rotationDuration = 0.5f; // Тривалість обертання
    [SerializeField] private Ease rotationEase = Ease.OutQuad; // Тип easing для обертання
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
        RotateHandle(-35); // Плавне обертання на 35 градусів
    }

    public void Off()
    {
        gate.CloseGate();
        RotateHandle(35); // Повернення до початкового положення
    }
    private void RotateHandle(float targetAngle)
    {
        // Плавно обертаємо switchHandle до заданого кута
        switchHandle.DOLocalRotate(new Vector3(0, 0, targetAngle), rotationDuration)
            .SetEase(rotationEase); // Використовуємо легку анімацію
    }
}
