using UnityEngine;

public class SphereSkill : TargetedSkill
{
    public ShieldSegmentManager spheres;
    public float speed = 5f;
    public float fixedDistance = 6f;

    protected override void UseSkillAtPosition(Vector3 position)
    {
        Transform sphere;
        sphere = spheres.segments[0];
        if (sphere == null) return;
        spheres.segments.Remove(sphere);
        sphere.parent = null;
        Vector3 direction = (position - sphere.position).normalized;
        Vector3 targetPoint = sphere.position + direction * fixedDistance;
        ThroveSphere(targetPoint, sphere);
    }
    protected virtual void ThroveSphere(Vector3 position, Transform sphere)
    {
        StartCoroutine(MoveSphereCoroutine(sphere, position, speed));
    }
    private System.Collections.IEnumerator MoveSphereCoroutine(Transform sphere, Vector3 target, float speed)
    {
        Vector3 start = sphere.position;
        float distance = Vector3.Distance(start, target);
        float t = 0f;
        float duration = distance / speed;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float easedT = Mathf.SmoothStep(0, 1, t); // ease-in-out
            sphere.position = Vector3.Lerp(start, target, easedT);
            yield return null;
        }
        sphere.position = target;
        sphere.GetComponent<AltProjectile>().ReturnToPool();
    }
}
