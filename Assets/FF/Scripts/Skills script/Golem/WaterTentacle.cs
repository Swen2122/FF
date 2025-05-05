using UnityEngine;

public class WaterTentacle : TargetedSkill
{
    public TectacleSO tentacleSettings;
    public float range;
    GameObject target;
    protected override void UseSkillAtPosition(Vector3 position)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(position);
        if (hitCollider != null)
        {
            target = hitCollider.gameObject;
            TentacleFactory.Instance.CreateTentacle(tentacleSettings, gameObject.transform, target.transform, range, mainCamera);
        }
        
    }
    void Update()
    {
        if(PauseManager.IsPaused) return;
        if (target != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 pushDirection = (mousePosition - (Vector2)target.transform.position).normalized;
                float distance = Vector2.Distance(transform.position, target.transform.position);
                Rigidbody2D targetRbody = target.GetComponent<Rigidbody2D>();
                if (targetRbody != null && distance <= 10)
                {
                    float forceMultiplier = Mathf.Clamp01((10 - distance) / 10);
                    targetRbody.AddForce(pushDirection * 400 * forceMultiplier, ForceMode2D.Impulse);
                }
            }
        }
    }

}