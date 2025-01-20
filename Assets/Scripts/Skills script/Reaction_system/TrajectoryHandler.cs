using UnityEngine;
using DG.Tweening;
using System;

public class TrajectoryHandler
{
    private readonly Transform transform;
    private readonly ProjectileData data;
    private Tween currentTween;
    private bool isPaused;

    public event Action<Vector3> OnPositionChanged;
    public event Action<float> OnProgressChanged;

    public TrajectoryHandler(Transform transform, ProjectileData data)
    {
        this.transform = transform;
        this.data = data;
        isPaused = false;
    }

    // Базовий лінійний рух
    public void MoveLinear(Vector2 targetPosition, TweenCallback onComplete)
    {
        KillCurrentTween();
        float duration = Vector2.Distance(transform.position, targetPosition) / data.speed;

        currentTween = transform.DOMove(targetPosition, duration)
            .SetEase(data.moveEase)
            .OnComplete(onComplete)
            .OnUpdate(() => {
                OnPositionChanged?.Invoke(transform.position);
                OnProgressChanged?.Invoke(currentTween.ElapsedPercentage());
            });
    }

    // Параболічний рух для артилерійських траєкторій
    public void MoveParabolic(Vector2 targetPosition, float height, TweenCallback onComplete)
    {
        KillCurrentTween();
        float duration = Vector2.Distance(transform.position, targetPosition) / data.speed;
        Vector2 startPos = transform.position;

        currentTween = DOTween.To(
            () => 0f,
            (float progress) => {
                float x = Mathf.Lerp(startPos.x, targetPosition.x, progress);
                float y = startPos.y + (targetPosition.y - startPos.y) * progress
                    + height * Mathf.Sin(progress * Mathf.PI);
                transform.position = new Vector3(x, y, transform.position.z);

                OnPositionChanged?.Invoke(transform.position);
                OnProgressChanged?.Invoke(progress);
            },
            1f,
            duration
        ).SetEase(data.moveEase)
        .OnComplete(onComplete);
    }
    // Рух для кривої траекторії
    public void MoveCurve(Vector2 targetPosition, float height, TweenCallback onComplete)
    {
        KillCurrentTween();
        float duration = Vector2.Distance(transform.position, targetPosition) / data.speed;
        Vector2 startPos = transform.position;

        // Визначаємо основний напрямок руху
        Vector2 direction = (targetPosition - startPos).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        currentTween = DOTween.To(
            () => 0f,
            (float progress) =>
            {
            // Базова позиція на прямій між стартом і ціллю
            Vector2 basePosition = Vector2.Lerp(startPos, targetPosition, progress);

            // Крива висоти: підйом до max і плавний спад
            float peakOffset = Mathf.Sin(progress * Mathf.PI) * height;

            // Додаємо зміщення перпендикулярно до напрямку руху
            Vector2 offset = perpendicular * peakOffset;
                Vector2 finalPosition = basePosition + offset;

                transform.position = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);

                OnPositionChanged?.Invoke(transform.position);
                OnProgressChanged?.Invoke(progress);
            },
            1f,
            duration
        )
        .SetEase(data.moveEase)
        .OnComplete(onComplete);
    }
    // Зміїний рух
    public void MoveSnake(Vector2 targetPosition, float amplitude, float frequency, TweenCallback onComplete)
    {
        KillCurrentTween();
        float duration = Vector2.Distance(transform.position, targetPosition) / data.speed;
        Vector2 startPos = transform.position;
        Vector2 direction = (targetPosition - startPos).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        currentTween = DOTween.To(
            () => 0f,
            (float progress) => {
                Vector2 basePosition = Vector2.Lerp(startPos, targetPosition, progress);
                Vector2 offset = perpendicular * amplitude * Mathf.Sin(progress * frequency * Mathf.PI * 2);
                transform.position = basePosition + offset;

                OnPositionChanged?.Invoke(transform.position);
                OnProgressChanged?.Invoke(progress);
            },
            1f,
            duration
        ).SetEase(data.moveEase)
        .OnComplete(onComplete);
    }

    // Керування рухом
    public void Pause()
    {
        if (currentTween != null && currentTween.IsPlaying())
        {
            currentTween.Pause();
            isPaused = true;
        }
    }

    public void Resume()
    {
        if (currentTween != null && isPaused)
        {
            currentTween.Play();
            isPaused = false;
        }
    }

    public void Kill()
    {
        KillCurrentTween();
    }

    private void KillCurrentTween()
    {
        if (currentTween != null && currentTween.IsPlaying())
        {
            currentTween.Kill();
            isPaused = false;
        }
    }
}