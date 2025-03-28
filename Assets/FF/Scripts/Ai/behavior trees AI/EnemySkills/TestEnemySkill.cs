using UnityEngine;

public class TestEnemySkill : BaseSkills
{
    protected override void UseSkill()
    {
        Vector2 shoterPosition = gameObject.transform.position;
        Vector2 targetPosition = shoterPosition + Vector2.right * 10;
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        rb.AddForce(targetPosition, ForceMode2D.Impulse);

    }
}
