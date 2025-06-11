using UnityEngine;

public interface IAnimated
{ 
    void Animate(float deltaTime);
    void AnimateTo(Vector3 targetPosition, float deltaTime); // якщо треба анімувати до точки
    void ResetAnimation();
    void SetAnimationState(string state);
} 
