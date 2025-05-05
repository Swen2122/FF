using UnityEngine;

public class UseSkillPosition : EnemyBehaviorNode
{
    private TriggerType triggerType;
    private Vector3 targetPosition;

    public UseSkillPosition(BaseEnemyAI enemyAI, Vector3 targetPosition, TriggerType triggerType) : base(enemyAI)
    {
        this.triggerType = triggerType;
        this.targetPosition = targetPosition;
    }
    public override NodeState Evaluate()
    {
        enemy.enemySkillManager.ActivateTrigger(triggerType);
        state = NodeState.Success;
        return state;
    }
}
