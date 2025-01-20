// Композитний вузол - Sequence (виконується поки всі не повернуть Success)
public class SequenceNode : Node
{
    public SequenceNode() : base() { }

    public override NodeState Evaluate()
    {
        bool isAnyNodeRunning = false;

        foreach (Node node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Running:
                    isAnyNodeRunning = true;
                    continue;
                case NodeState.Success:
                    continue;
                case NodeState.Failure:
                    state = NodeState.Failure;
                    return state;
            }
        }

        state = isAnyNodeRunning ? NodeState.Running : NodeState.Success;
        return state;
    }
}
