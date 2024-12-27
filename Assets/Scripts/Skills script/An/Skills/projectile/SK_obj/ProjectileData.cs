using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
// Налаштування окремого снаряда
[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Skills/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Basic Settings")]
    public GameObject defaultProjectilePrefab; // Базовий префаб для випадку None елементу
    public float damage = 10f;

    [Header("Elemental Settings")]
    // Словник для зберігання префабів елементальних снарядів
    private Dictionary<Element, GameObject> elementalProjectiles = new Dictionary<Element, GameObject>();

    // Серіалізовані поля для інспектора
    [SerializeField] private GameObject fireProjectilePrefab;
    [SerializeField] private GameObject waterProjectilePrefab;
    [SerializeField] private GameObject earthProjectilePrefab;
    [SerializeField] private GameObject airProjectilePrefab;
    [SerializeField] private GameObject iceProjectilePrefab;
    [SerializeField] private GameObject electroProjectilePrefab;

    [Header("Movement Settings")]
    public float speed = 20f;
    public float range = 10f;
    public Ease moveEase = Ease.OutExpo;

    [Header("Parabolic Movement")]
    public bool useParabolicPath = false;
    public float parabolaHeight = 2f;
    public bool useConvergence = false;
    public float convergenceOffset = 0.5f;

    [Header("Visual Effects")]
    public GameObject spawnEffect;
    public GameObject hitEffect;
    public GameObject trailEffect;

    [Header("Screen Shake")]
    public bool useScreenShake = false;
    public float shakeStrength = 0.5f;
    public float shakeDuration = 0.1f;

    [Header("Sound")]
    public AudioClip launchSound;
    public AudioClip hitSound;

    // Ініціалізація словника при завантаженні ScriptableObject
    private void OnEnable()
    {
        elementalProjectiles.Clear();
        elementalProjectiles[Element.Fire] = fireProjectilePrefab;
        elementalProjectiles[Element.Water] = waterProjectilePrefab;
        elementalProjectiles[Element.Earth] = earthProjectilePrefab;
        elementalProjectiles[Element.Wind] = airProjectilePrefab;
        elementalProjectiles[Element.Ice] = iceProjectilePrefab;
        elementalProjectiles[Element.Electro] = electroProjectilePrefab;
    }

    // Метод для отримання префабу снаряду на основі елементу
    public GameObject GetProjectilePrefab(Element element)
    {
        if (element == Element.None || !elementalProjectiles.ContainsKey(element))
            return defaultProjectilePrefab;

        return elementalProjectiles[element] ?? defaultProjectilePrefab;
    }
}