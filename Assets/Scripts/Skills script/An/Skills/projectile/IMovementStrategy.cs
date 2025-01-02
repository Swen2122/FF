using UnityEngine;
using DG.Tweening;
using System;
// Інтерфейс для стратегії руху
public interface IMovementStrategy
{
    void Initialize(Transform transform, Vector2 targetPosition, float speed, Ease ease);
    void StartMovement(Action onComplete);

}
