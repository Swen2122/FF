using UnityEngine;

// Налаштування конвергенції
[CreateAssetMenu(fileName = "New Vortex Settings", menuName = "Element/Vortex Settings")]
public class VortexSettings : ScriptableObject
{
    [Header("Налаштування затягування")]
    public float pullForce = 5f;        // Сила затягування
    public float pullRadius = 5f;       // Радіус затягування
    public float pullInterval = 0.1f;   // Частотта притягування
}
