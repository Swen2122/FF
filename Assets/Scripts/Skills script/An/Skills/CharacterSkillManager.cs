using UnityEngine;
public class CharacterSkillManager : MonoBehaviour
{
    [System.Serializable]
    public class SkillBinding
    {
        public KeyCode key;
        public BaseSkills skill;
        public bool requiresPosition;
        public BaseSkills chargeSkill;
        public bool chargeRequiresPosition;
        public float chargeTime;
        [HideInInspector] public float chargeStartTime;
        [HideInInspector] public bool isCharging;
    }

    [SerializeField] private SkillBinding[] skillBindings;
    [SerializeField] private SkillChargeIndicator chargeIndicator;
   private void Update()
    {
        foreach (var binding in skillBindings)
        {
            if (binding.skill == null) continue;

            if (Input.GetKeyDown(binding.key))
            {
                if (binding.chargeSkill != null)
                {
                    StartCharging(binding);
                }
                else
                {
                    ExecuteSkill(binding);
                }
            }

            if (binding.isCharging)
            {
                if (Input.GetKey(binding.key))
                {
                    UpdateCharging(binding);
                }
                else if (Input.GetKeyUp(binding.key))
                {
                    ExecuteChargedSkill(binding);
                }
            }
        }
    }

    private void StartCharging(SkillBinding binding)
    {
        if (!binding.skill.CanUseSkill()) return;
        chargeIndicator.ShowIndicator(true);
        binding.isCharging = true;
        binding.chargeStartTime = Time.time;
    }
    private void ExecuteSkill(SkillBinding binding)
    {
        if (binding.requiresPosition)
        {
            (binding.skill as TargetedSkill)?.TryUseSkillAtPosition();
        }
        else
        {
            binding.skill.TryUseSkill();
        }
    }
    private void UpdateCharging(SkillBinding binding)
    {
        float currentChargeTime = Time.time - binding.chargeStartTime;
        float chargePercent = Mathf.Clamp01(currentChargeTime / binding.chargeTime);
        chargeIndicator.UpdateCharge(chargePercent);
    }
    private void ExecuteChargedSkill(SkillBinding binding)
    {
        float currentChargeTime = Time.time - binding.chargeStartTime;
        float chargePercent = Mathf.Clamp01(currentChargeTime / binding.chargeTime);
        if (chargePercent >= 1f)
        {
            if (binding.chargeRequiresPosition)
            {
                (binding.chargeSkill as TargetedSkill)?.TryUseSkillAtPosition();
            }
            else
            {
                binding.chargeSkill.TryUseSkill();
            }
        } else ExecuteSkill(binding);
        chargeIndicator.ShowIndicator(false);
        binding.isCharging = false;
    }
}
