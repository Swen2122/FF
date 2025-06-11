using UnityEngine;

public class AltProjectile : MonoBehaviour
{
    private ShieldSegmentManager segmentManager;
    
    public void Initialize(ShieldSegmentManager sm)
    {
        segmentManager = sm;
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (segmentManager != null)
        {
            Debug.Log("Collision: " + name);
            segmentManager.ReturnSphere(GetComponent<ReactionItem>());
        }
    }
    public void ReturnToPool()
    {
        if (segmentManager != null)
        {
            segmentManager.ReturnSphere(GetComponent<ReactionItem>());
        }
    }
}
