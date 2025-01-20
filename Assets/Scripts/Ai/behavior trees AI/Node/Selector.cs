// Композитний вузол - Selector (виконується поки хтось не поверне Success)
public class Selector : Node
{
    public Selector() : base() { }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Success:
                    state = NodeState.Success;
                    return state;
                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
                case NodeState.Failure:
                    continue;
            }
        }
        state = NodeState.Failure;
        return state;
    }
}
