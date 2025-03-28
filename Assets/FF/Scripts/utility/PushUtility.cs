using UnityEngine;

public static class PushUtility
{
    public static void Push(Rigidbody2D target, Vector2 origin, float pushforce)
    {
        Vector2 push_dir = ((Vector2)target.transform.position - (Vector2)origin).normalized;
        target.linearVelocity = Vector2.zero;
        target.AddForce(push_dir * pushforce, ForceMode2D.Impulse);
    }
}
