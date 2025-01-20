using UnityEngine;

public class WaterBarrel : DestroyebleObject
{
    [Header("Water Settings")]
    [SerializeField] private float waterAmount = 5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float waterPressure = 3f;
    [SerializeField] private int waterPoints = 12;
    
    [Header("Stream Settings")]
    [SerializeField] private bool createStream = true;
    [SerializeField] private float streamDuration = 3f;
    [SerializeField] private float streamRate = 20f;

    private WaterSystem waterSystem;

    protected override void Start()
    {
        base.Start(); // ��������� Start �������� �����

        // ��������� WaterSystem � ����
        waterSystem = FindObjectOfType<WaterSystem>();
        if (waterSystem == null)
        {
            Debug.LogError("WaterSystem not found in scene!", this);
        }
    }

    public override void TakeHit(float damage, Element element)
    {
        // ��������� ������ � ����� ���������
        if (waterSystem != null)
        {
            Vector2 hitDirection = Random.insideUnitCircle.normalized;
            waterSystem.CreateWaterSplash(
                position: transform.position,
                direction: hitDirection,
                spread: 45f,
                drops: 3
            );
        }

        base.TakeHit(damage, element); // ��������� ������ �����
    }

    protected override void DestroyObject()
    {
        if (waterSystem != null)
        {
            // ��������� ����� ����
            waterSystem.CreateWaterExplosion(
                position: transform.position,
                radius: explosionRadius,
                amount: waterAmount / waterPoints,
                pressure: waterPressure,
                points: waterPoints
            );

            // ���� �������, ��������� ���� ����
            if (createStream)
            {
                StartCoroutine(waterSystem.CreateWaterStream(
                    position: transform.position,
                    duration: streamDuration,
                    ratePerSecond: streamRate,
                    amount: waterAmount / (streamRate * streamDuration),
                    pressure: waterPressure * 0.5f
                ));
            }
        }

        base.DestroyObject(); // ��������� ������ ����� ����������
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}