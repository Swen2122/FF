using UnityEngine;
using DG.Tweening;
public class TrajectoryHandler
{
    private readonly Transform transform;
    private readonly ProjectileData data;

    public TrajectoryHandler(Transform transform, ProjectileData data)
    {
        this.transform = transform;
        this.data = data;
    }

    public void MoveLinear(Vector2 targetPosition, TweenCallback onComplete)
    {
        float duration = Vector2.Distance(transform.position, targetPosition) / data.speed;
        transform.DOMove(targetPosition, duration).SetEase(data.moveEase).OnComplete(onComplete);
    }
}
