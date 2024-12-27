using UnityEngine;
public class CharacterSkillManager : MonoBehaviour
{
    [System.Serializable]
    public class SkillBinding
    {
        public KeyCode key;
        public BaseSkills skill;
        public bool requiresPosition;
    }

    [SerializeField] private SkillBinding[] skillBindings;

    private void Update()
    {
        foreach (var binding in skillBindings)
        {
            if (Input.GetKeyDown(binding.key))
            {
                if (binding.requiresPosition)
                {
                    (binding.skill as TargetedSkill)?.TryUseSkillAtPosition();
                }
                else
                {
                    binding.skill?.TryUseSkill();
                }
            }
        }
    }
}
