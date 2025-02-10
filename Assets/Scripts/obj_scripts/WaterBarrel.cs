using UnityEngine;

public class WaterBarrel : DestroyebleObject
{
    [Header("Water Settings")]
    [SerializeField] private float waterAmount = 5f;

    private GradualWaterSystem waterSystem;
    protected override void Start()
    {
        
        base.Start(); // ��������� Start �������� �����
        waterSystem = Object.FindFirstObjectByType<GradualWaterSystem>();
    }

    public override void TakeHit(float damage, Element element)
    {

        base.TakeHit(damage, element); // ��������� ������ �����
    }

    protected override void DestroyObject()
    {
        Vector2 position = transform.position;
        waterSystem?.InitiateWaterSpread(position, waterAmount);
        base.DestroyObject(); // ��������� ������ ����� ����������
    }
}