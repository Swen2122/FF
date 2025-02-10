using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "NewConvergenceSkillData", menuName = "Skills/ConvergenceSkill Data")]
public class SkillData : ScriptableObject
{
    public GameObject projectilePrefab;  // ������ �������
    public GameObject impactPrefab;      // ������ ��'���� ��� ��������
    public float projectileSpeed = 5f;   // �������� �������
    public float curveAmount  = 2f;    // ������ ��������
    public float maxRange = 10f;         // ����������� ��������� �������
    public Ease movementEase = Ease.Linear; // ��� ����
}
