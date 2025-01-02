using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
// Налаштування окремого снаряда
[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Skills/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Basic Settings")]
    public GameObject defaultProjectilePrefab; // Префаб для None елементу
    public float damage = 10f;

    [Header("Elemental Prefabs")]
    [SerializeField] private GameObject fireProjectilePrefab;
    [SerializeField] private GameObject waterProjectilePrefab;
    [SerializeField] private GameObject earthProjectilePrefab;
    [SerializeField] private GameObject airProjectilePrefab;
    [SerializeField] private GameObject iceProjectilePrefab;
    [SerializeField] private GameObject electroProjectilePrefab;

    private Dictionary<Element, GameObject> elementalProjectiles;

    [Header("Movement Settings")]
    public float speed = 20f;
    public Ease moveEase = Ease.OutExpo;
    public float range = 10f;
    public float parabolaHeight = 1f;
    public enum ProjectileType
    {
        Basic,
        Chain,
        Explosive,
        Pulling
    }

    [Header("Projectile Type")]
    public ProjectileType projectileType;
    [Header("Visual Effects")]
    public GameObject spawnEffect;
    public GameObject hitEffect;
    public GameObject trailEffect;

    [Header("Sound Effects")]
    public AudioClip launchSound;
    public AudioClip hitSound;
    private void OnEnable()
    {
        elementalProjectiles = new Dictionary<Element, GameObject>
        {
            { Element.Fire, fireProjectilePrefab },
            { Element.Water, waterProjectilePrefab },
            { Element.Earth, earthProjectilePrefab },
            { Element.Wind, airProjectilePrefab },
            { Element.Ice, iceProjectilePrefab },
            { Element.Electro, electroProjectilePrefab }
        };
    }

    public GameObject GetProjectilePrefab(Element element)
    {
        return elementalProjectiles.TryGetValue(element, out var prefab) && prefab != null ? prefab : defaultProjectilePrefab;
    }
}
