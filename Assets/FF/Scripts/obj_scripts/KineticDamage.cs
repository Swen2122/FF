using System.Threading.Tasks;
using UnityEngine;

public class KineticDamage : MonoBehaviour
{
    private Rigidbody2D rb;
    private float damage = 0.0f;
    public float minSpeed = 1.0f;
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(rb.linearVelocity.magnitude > minSpeed)
        {
            damage = rb.linearVelocity.magnitude * rb.mass;
            if (collision.gameObject.TryGetComponent<ICanHit>(out var canHit))
            {
                canHit.TakeHit(damage, Element.Earth);
            }
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out var targetRb))
            {
                Vector2 pullDirection = (transform.position - collision.transform.position).normalized;
                targetRb.AddForce(pullDirection * rb.linearVelocity.magnitude, ForceMode2D.Impulse);
            }
        } 
    }
}
