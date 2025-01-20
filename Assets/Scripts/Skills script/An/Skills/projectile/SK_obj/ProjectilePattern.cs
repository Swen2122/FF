using UnityEngine;
// Налаштування патерну стрільби
[CreateAssetMenu(fileName = "New Projectile Pattern", menuName = "Skills/Projectile Pattern")]
public class ProjectilePattern : ScriptableObject
{
    [Header("Pattern Type")]
    public bool isBurst;
    public bool hasSpread;
    public bool useConvergence;

    [Header("Convergence Type")]
    public bool useConvergencePoints = false;
    public bool useSymmetricalPaths = true;

    [Header("Basic Pattern")]
    public int projectilesCount = 1;
    public float angleBetweenProjectiles = 15f;

    [Header("Spread Settings")]
    public float spreadAngle = 15f;

    [Header("Burst Settings")]
    public int burstCount = 3;
    public float burstDelay = 0.1f;

    [Header("Convergence Settings")]
    public float curveHeight = 2f;
    public float convergenceOffset = 2f;
    public bool isHorizontalDominant = true;
}