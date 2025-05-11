using UnityEngine;

public class WaterCheck : MonoBehaviour
{
    public int samplePoints = 8;
    public CircleCollider2D playerCircle;
    [Range(0, 1)]public float fillPercent = 1f;
    public LayerMask waterLayer;
    public SpriteRenderer playerSprite;
    public CharterStats playerStats;
    private Vector2[] offsets;
    public bool isInWater = false;
    private bool isToDrow = false;
    private Coroutine drowCoroutine;

    void Awake() 
    {
        if (playerCircle == null) 
        {
            playerCircle = GetComponent<CircleCollider2D>();
            if (playerCircle == null) 
            {
                Debug.LogError("No CircleCollider2D found on this GameObject or its children.");
                return;
            }
        }
        offsets = new Vector2[samplePoints];
        float r = playerCircle.radius;
        for(int i = 0; i < samplePoints; i++) 
        {
            float ang = 2 * Mathf.PI * i / samplePoints;
            offsets[i] = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * r;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((waterLayer.value & (1 << other.gameObject.layer)) == 0) return;
        isInWater = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if ((waterLayer.value & (1 << other.gameObject.layer)) == 0) return;
        isInWater = false;
        Drow(false);
    }
    private void Update()
    {
        if (!isInWater) return;
        float ratio = CalculateSubmergeRatio();
        if (ratio >= fillPercent)
        {
            Drow(true);
            Debug.Log($"Submerged! Ratio = {ratio * 100f:F1}%");
        } else Drow(false);

    }
    private float CalculateSubmergeRatio()
    {
        Vector2 center = (Vector2)transform.position + playerCircle.offset * (Vector2)transform.localScale;
        int insideCount = 0;
        for (int i = 0; i < samplePoints; i++)
        {
            Vector2 samplePoint = center + offsets[i];
            if (Physics2D.OverlapPoint(samplePoint, waterLayer) != null)
                insideCount++;
        }
        return (float)insideCount / samplePoints;
    }
    private void Drow(bool isDrow)
    {
        isToDrow = isDrow;
        if(playerStats)
        playerStats.controler.CanDash = !isToDrow;
        playerStats.SetSpeedModifier(isToDrow ? 0.1f : 1f);
        if(playerSprite)
        playerSprite.color = isToDrow ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
        if (isToDrow)
        {
            drowCoroutine ??= StartCoroutine(DrowCoroutine());
        }
        else
        {
            if (drowCoroutine != null)
            {
                StopCoroutine(drowCoroutine);
                drowCoroutine = null;
            }
        }
    }

    private System.Collections.IEnumerator DrowCoroutine()
    {
        yield return new WaitForSeconds(5f);
        while (isToDrow)
        {
            playerStats.health.TakeHit(5f, Element.Water);
            yield return new WaitForSeconds(0.5f);
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (playerCircle == null) return;
        if (offsets == null || offsets.Length != samplePoints)
        {
            offsets = new Vector2[samplePoints];
            float r = playerCircle.radius;
            for (int i = 0; i < samplePoints; i++)
            {
                float ang = 2 * Mathf.PI * i / samplePoints;
                offsets[i] = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * r;
            }
        }

        Gizmos.color = Color.blue;
        Vector2 center = (Vector2)playerCircle.transform.position + playerCircle.offset;

        for (int i = 0; i < samplePoints; i++)
        {
            Vector2 point = center + offsets[i];
            Gizmos.DrawSphere(point, 0.05f);
        }
    }
#endif
}
