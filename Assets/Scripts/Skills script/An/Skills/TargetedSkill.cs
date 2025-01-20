using UnityEngine;

public abstract class TargetedSkill : BaseSkills
{
    public void TryUseSkillAtPosition()
    {
        if (!CanUseSkill()) return;

        Vector2 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        UseSkillAtPosition(targetPosition);
        lastUseTime = Time.time;
        PlaySkillEffects();
    }

    protected abstract void UseSkillAtPosition(Vector2 position);

    // ������������� ������� �����, ��� �� �� ����������������
    protected override void UseSkill()
    {
        throw new System.NotImplementedException();
    }
}
