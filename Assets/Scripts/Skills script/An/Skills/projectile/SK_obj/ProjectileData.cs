using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
// Налаштування окремого снаряда
[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Skills/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Basic Settings")]
    public GameObject projectilePrefab;
    public float damage = 10f;

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
        Grappling,
        Knockback
    }

    [Header("Projectile Type")]
    public bool canTriggerReaction = true;
    public ProjectileType projectileType;
    [Header("Visual Effects")]
    public GameObject spawnEffect;
    public GameObject hitEffect;
    public GameObject trailEffect;

    [Header("Sound Effects")]
    public AudioClip launchSound;
    public AudioClip hitSound;
}
