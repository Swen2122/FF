using UnityEngine;
// ������������ ������� �������
[CreateAssetMenu(fileName = "New Projectile Pattern", menuName = "Skills/Projectile Pattern")]
public class ProjectilePattern : ScriptableObject
{
    [Header("Pattern Type")]
    public bool isBurst;
    public bool hasSpread;

    [Header("Basic Pattern")]
    public int projectilesCount = 1;
    public float angleBetweenProjectiles = 15f;

    [Header("Spread Settings")]
    public float spreadAngle = 15f;

    [Header("Burst Settings")]
    public int burstCount = 3;
    public float burstDelay = 0.1f;

    [Header("Misc Settings")]
    public float curveHeight = 2f;
}