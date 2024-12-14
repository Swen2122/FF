using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "NewConvergenceSkillData", menuName = "Skills/ConvergenceSkill Data")]
public class SkillData : ScriptableObject
{
    public GameObject projectilePrefab;  // Префаб снаряду
    public GameObject impactPrefab;      // Префаб об'єкта при зіткненні
    public float projectileSpeed = 5f;   // Швидкість снарядів
    public float parabolaHeight = 2f;    // Висота параболи
    public float maxRange = 10f;         // Максимальна дальність навички
    public Ease movementEase = Ease.Linear; // Тип руху
}
