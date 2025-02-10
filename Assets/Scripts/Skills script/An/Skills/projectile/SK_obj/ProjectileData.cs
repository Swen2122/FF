using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
// ������������ �������� �������
[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Skills/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Basic Settings")]
    public GameObject projectilePrefab;
    public float damage = 10f;

    [Header("Movement Settings")]
    public float speed = 20f;
    public float range = 10f;
    
    [Header("Projectile Type")]
    public bool canTriggerReaction = true;
    
    [Header("Visual Effects")]
    public GameObject spawnEffect;
    public GameObject hitEffect;
    public GameObject trailEffect;

    [Header("Sound Effects")]
    public AudioClip launchSound;
    public AudioClip hitSound;
}
