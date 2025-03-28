using System.Collections.Generic;
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
    [System.Serializable]
    public class Projectile
    {
        public ProjectileData projectileData;
        public ProjectilePattern pattern;
        public Element elementType;
    }
    public List<Projectile> projectiles = new List<Projectile>();
    [Header("Projectile Settings")]
    public ProjectileData projectileData;
    public ProjectilePattern pattern;
    public ElementData elementData;
    public GameObject GetProjectileData(Element element)
    {
        foreach (var proj in projectiles)
        {
            if (proj.elementType == element)
            {
                return proj.projectileData.projectilePrefab;
            }
        }

        Debug.LogWarning($"No projectile data found for element {element}. Returning default.");
        return null;
    }
}