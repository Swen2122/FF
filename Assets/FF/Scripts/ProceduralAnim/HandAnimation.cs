using System.Collections;
using UnityEngine;

public class HandAnimation : MonoBehaviour, IAnimated
{
    public Vector3 target; // Цільова точка для анімації
    public float speed = 1.0f; // Швидкість анімації
    private bool isAnimating = false; // Прапорець для перевірки, чи анімація активна

    private Vector3 initialPosition;
    public void Inicialize(Vector3 target)
    {
        this.target = target;
    }
    void Start()
    {
        initialPosition = transform.position;
    }

    public void Animate(float deltaTime)
    {
        if (isAnimating) return;
        float t = Mathf.Clamp01(speed * deltaTime);
        float easedT = GetEase.EaseOutElastic(t);
        transform.position = Vector3.Lerp(transform.position, target, easedT);
    }

    public void AnimateTo(Vector3 targetPosition, float deltaTime)
    {
        float t = Mathf.Clamp01(speed * deltaTime);
        float easedT = GetEase.EaseOutElastic(t);
        transform.position = Vector3.Lerp(transform.position, targetPosition, easedT);
    }

    public void ResetAnimation()
    {
        // Скидання анімації до початкової позиції
        transform.position = initialPosition;
    }

    public System.Action<HandAnimation> OnAttackComplete;

    public void StartAnimation(Vector3 targetPosition)
    {
        StartCoroutine(AnimCoroutine(targetPosition));
    }

    private IEnumerator AnimCoroutine(Vector3 targetPosition)
    {
        isAnimating = true;
        Vector3 start = transform.position;
        float duration = 0.2f;
        float timer = 0f;

        // Рух до точки удару
        while (timer < duration)
        {
            float t = timer / duration;
            float easedT = GetEase.EaseOutElastic(t);
            transform.position = Vector3.Lerp(start, targetPosition, easedT);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        // Рух назад на свою позицію
        timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            float easedT = GetEase.EaseOutElastic(t);
            transform.position = Vector3.Lerp(targetPosition, this.target, easedT);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = this.target;

        isAnimating = false;
        OnAttackComplete?.Invoke(this);
    }

    public void SetAnimationState(string state)
    {
        // Встановлення стану анімації (можна реалізувати логіку для різних станів)
        Debug.Log("Animation state set to: " + state);
    }
}
