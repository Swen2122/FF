using UnityEngine;
using System.Collections.Generic;
public class SkillGroup : BaseSkills
{
     [System.Serializable]
    public class SkillElementPair
    {
        public BaseSkills skill;
        public Element element;
    }
    [SerializeField]
    private List<SkillElementPair> skillElementPairs = new List<SkillElementPair>();

    public BaseSkills GetSkillByElement(Element element)
    {
        foreach (var pair in skillElementPairs)
        {
            if (pair.element == element)
            {
                return pair.skill;
            }
        }
        Debug.LogWarning($"Skill with element {element} not found!");
        return null;
    }
    protected override void UseSkill()
    {
        if (element != null)
        {
            BaseSkills skill = GetSkillByElement(element.currentElement);

            if (skill != null)
            {
                if(skill is TargetedSkill target)
                {
                    target.TryUseSkillAtPosition();
                } else
                {
                    skill.TryUseSkill();
                }
            }
        }
    }
}
