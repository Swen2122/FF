using UnityEngine;

public class ArcherSkill : MonoBehaviour
{
    public EnemyShoot shoot;
    [SerializeReference] private ProjectileSkillData bullets;
    public void BoltShot(Transform target,float speed, int damage)
    {
    }
    private void Charge()
    {

    }
}
