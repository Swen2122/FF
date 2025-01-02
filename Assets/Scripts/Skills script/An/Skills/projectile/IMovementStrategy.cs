using UnityEngine;
using DG.Tweening;
using System;
// ��������� ��� ������㳿 ����
public interface IMovementStrategy
{
    void Initialize(Transform transform, Vector2 targetPosition, float speed, Ease ease);
    void StartMovement(Action onComplete);

}
