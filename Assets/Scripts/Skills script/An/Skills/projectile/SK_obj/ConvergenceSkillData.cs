using UnityEngine;

// Налаштування конвергенції
[CreateAssetMenu(fileName = "New Convergence Settings", menuName = "Skills/Convergence Settings")]
public class ConvergenceSkillData : ScriptableObject
{
    [Header("Vortex Settings")]
    public GameObject vortexPrefab;
    public float vortexLifetime = 5f;

    [Header("Pull Settings")]
    public float pullForce = 5f;
    public float pullRadius = 5f;

    [Header("Damage Settings")]
    public float damageRadius = 3f;
    public float damageInterval = 0.5f;
    public int damageAmount = 10;

    [Header("Visual Effects")]
    public GameObject convergenceEffect;
    public ParticleSystem pullEffect;

    [Header("Audio")]
    public AudioClip convergenceSound;
    public AudioClip pullSound;
    public AudioClip destroySound;
}
