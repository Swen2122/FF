using UnityEngine;

public class ActionTriger : EnemyBehaviorNode
{
    private TriggerType triggerType;

    public ActionTriger(BaseEnemyAI enemyAI, TriggerType triggerType) : base(enemyAI)
    {
        this.triggerType = triggerType;
    }

    public override NodeState Evaluate()
    {
        enemy.enemySkillManager.ActivateTrigger(triggerType);
        state = NodeState.Success;
        return state;
    }
}
