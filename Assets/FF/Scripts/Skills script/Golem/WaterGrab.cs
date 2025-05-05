using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WaterGrab : TargetedSkill
{
    [SerializeField] private LayerMask targetLayer;     
    [SerializeField] private LayerMask obstacleLayer;   
    [SerializeField] private float maxDistance = 10f;   
    [SerializeField] private float pullForce = 5f;       
    [SerializeField] private GameObject bubble;        

    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    private Transform selectedTarget;
    private Rigidbody2D targetRigidbody;
    private GameObject spawnedBubble;

    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        springJoint = gameObject.AddComponent<SpringJoint2D>();
        springJoint.enabled = false;
        springJoint.autoConfigureDistance = false;
        springJoint.frequency = 1f;
        springJoint.dampingRatio = 0.5f;
    }

    void Update()
    {
        if (selectedTarget != null)
        {
            DrawLineToTarget();

            if (Input.GetMouseButtonDown(0))
            {
                PullTarget();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ReleaseTarget();
            }
        }
    }
    protected override void UseSkillAtPosition(Vector3 position)
    {
        PullTarget();
    }
    public void TryCaptureTarget()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, maxDistance, targetLayer);

        if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & targetLayer) != 0)
        {
            if (!Physics2D.Linecast(transform.position, hit.collider.transform.position, obstacleLayer))
            {
                selectedTarget = hit.collider.transform;
                targetRigidbody = selectedTarget.GetComponent<Rigidbody2D>();

                if (targetRigidbody != null)
                {
                    Bubble();
                    springJoint.enabled = true;
                    springJoint.connectedBody = targetRigidbody;
                    springJoint.distance = Vector2.Distance(transform.position, selectedTarget.position);
                    lineRenderer.enabled = true;
                }
            }
        }
    }

    private void Bubble()
    {
        if (selectedTarget != null && bubble != null)
        {
            if (spawnedBubble != null)
            {
                return;
            }

            spawnedBubble = Instantiate(bubble, selectedTarget.position, Quaternion.identity);
            spawnedBubble.transform.SetParent(selectedTarget);
        }
    }

    private void DrawLineToTarget()
    {
        if (selectedTarget != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, selectedTarget.position);
        }
    }

    private void PullTarget()
    {
        if (selectedTarget != null)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pushDirection = (mousePosition - (Vector2)selectedTarget.position).normalized;
            float distance = Vector2.Distance(transform.position, selectedTarget.position);

            if (targetRigidbody != null && distance <= maxDistance)
            {
                float forceMultiplier = Mathf.Clamp01((maxDistance - distance) / maxDistance);
                targetRigidbody.AddForce(pushDirection * pullForce * forceMultiplier, ForceMode2D.Impulse);
            }

            ReleaseTarget();
        }
    }

    private void ReleaseTarget()
    {
        selectedTarget = null;
        targetRigidbody = null;
        springJoint.enabled = false;
        lineRenderer.enabled = false;
    }
}