using UnityEngine;
public class CharacterSkillManager : MonoBehaviour
{
    [System.Serializable]
    public class SkillBinding
    {
        public KeyCode key;
        public TriggerType trigger = TriggerType.none;
        public BaseSkills skill;
        public float cost = 0f;
        public bool skipCostEnergy = false;
        public BaseSkills chargeSkill;
        public float chargeCost = 0f;
        public float chargeTime;
        [HideInInspector] public float chargeStartTime;
        [HideInInspector] public bool isCharging;
    }
    [SerializeField] protected Health health;
    [SerializeField] protected SkillBinding[] skillBindings;
    [SerializeField] protected SkillChargeIndicator chargeIndicator;
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
                    PlayerTracker.Instance.SetUseSkill();
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
                    PlayerTracker.Instance.SetUseSkill();
                    ExecuteChargedSkill(binding);
                }
            }
        }
    }
    public void ActivateTrigger(TriggerType triggerType)
    {
        foreach (var binding in skillBindings)
        {
            if (binding.trigger == triggerType)
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
        }
    }
    protected virtual void StartCharging(SkillBinding binding)
    {
        if (!binding.skill.CanUseSkill()) return;
        chargeIndicator.ShowIndicator(true);
        binding.isCharging = true;
        binding.chargeStartTime = Time.time;
    }
    protected virtual void ExecuteSkill(SkillBinding binding)
    {
        if (health != null && health.GetEnergy(Element.Wind) >= binding.cost)
        {
            if (binding.skill as TargetedSkill)
            {
                (binding.skill as TargetedSkill)?.TryUseSkillAtPosition();
            }
            else
            {
                binding.skill.TryUseSkill();
            }
            if(binding.skipCostEnergy) return;
            health.AddInternalEnergy(-binding.cost, Element.Wind);
        }
        
    }
    protected virtual void UpdateCharging(SkillBinding binding)
    {
        float currentChargeTime = Time.time - binding.chargeStartTime;
        float chargePercent = Mathf.Clamp01(currentChargeTime / binding.chargeTime);
        chargeIndicator.UpdateCharge(chargePercent);
    }
    protected virtual void ExecuteChargedSkill(SkillBinding binding)
    {
        float currentChargeTime = Time.time - binding.chargeStartTime;
        float chargePercent = Mathf.Clamp01(currentChargeTime / binding.chargeTime);
        if (chargePercent >= 1f)
        {
            if (health != null && health.GetEnergy(Element.Wind) >= binding.cost)
        {
            if (binding.chargeSkill as TargetedSkill)
            {
                (binding.chargeSkill as TargetedSkill)?.TryUseSkillAtPosition();
            }
            else
            {
                binding.chargeSkill.TryUseSkill();
            }
            health.AddInternalEnergy(-binding.cost, Element.Wind);
        }
        } else ExecuteSkill(binding);
        chargeIndicator.ShowIndicator(false);
        binding.isCharging = false;
    }
}
public enum TriggerType
{
    none,
    damage,
    heal,
    dash,
    projectile


}
