using UnityEngine;
public class CheckPlayer : EnemyBehaviorNode
{

    public bool playerAction;
    public PlayerAction playerActionType;
    public CheckPlayer(BaseEnemyAI enemyAI, PlayerAction action) : base(enemyAI)
    {
        playerActionType = action;
        Debug.Log("CheckPlayer: " + playerActionType);
    }
    public override NodeState Evaluate()
    {
        if(SetAction(playerActionType)) return NodeState.Success;

        state = NodeState.Failure;
        return state;
    }
    private bool SetAction(PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.Dash:
                playerAction = PlayerTracker.Instance.IsDashing;
                Debug.Log("Action.Dashing: " + playerAction);
                return playerAction;
            case PlayerAction.Skill:
                playerAction = PlayerTracker.Instance.IsUseSkill;
                return playerAction;
            default:
                playerAction = false;
                return playerAction;
        }
    }

}
public enum PlayerAction
{
    None,
    Dash,
    Move,
    Skill
}