using UnityEngine;

public abstract class TargetedSkill : BaseSkills
{
    protected Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    public void TryUseSkillAtPosition()
    {
        if (!CanUseSkill()) return;

        Vector2 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        UseSkillAtPosition(targetPosition);
        lastUseTime = Time.time;
        PlaySkillEffects();
    }

    protected abstract void UseSkillAtPosition(Vector2 position);

    // Перевизначаємо базовий метод, щоб він не використовувався
    protected override void UseSkill()
    {
        throw new System.NotImplementedException();
    }
}
