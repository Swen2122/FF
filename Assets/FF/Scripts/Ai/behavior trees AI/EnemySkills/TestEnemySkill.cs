using UnityEngine;

public class TestEnemySkill : BaseSkills
{
    protected override void UseSkill()
    {
        Debug.Log("UseSkill");
        Vector2 shoterPosition = gameObject.transform.position;
        Vector2 targetPosition = (Vector2)Controler.Instance.transform.position - shoterPosition;
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        rb.AddForce(targetPosition*10, ForceMode2D.Impulse);
    }
}
