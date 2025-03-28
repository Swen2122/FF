using UnityEngine;
public interface ISkill
{
    void Execute(Vector2 targetPosition);
    Element GetSkillElement();
}
