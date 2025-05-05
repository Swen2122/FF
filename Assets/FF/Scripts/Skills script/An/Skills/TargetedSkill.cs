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
    
    public void TryUseSkillAtPositionAI(Vector3 target)
    {
        if (!CanUseSkill()) return;
        //Vector2 targetPosition = target;
        UseSkillAtPosition(target);
        lastUseTime = Time.time;
        PlaySkillEffects();
    }

    protected abstract void UseSkillAtPosition(Vector3 position);

    protected override void UseSkill()
    {
        throw new System.NotImplementedException();
    }
}
