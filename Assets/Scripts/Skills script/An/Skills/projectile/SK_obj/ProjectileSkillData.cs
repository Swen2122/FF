using UnityEngine;

// Дані скіла
[CreateAssetMenu(fileName = "New Projectile Skill", menuName = "Skills/Projectile Skill Data")]
public class ProjectileSkillData : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;

    [Header("Base Settings")]
    public float cooldown = 1f;
    public float damageMultiplier = 1f;
    public string animationTrigger;

    [Header("Projectile Settings")]
    public ProjectileData projectileData;
    public ProjectilePattern pattern;
    public ElementData elementData;
}