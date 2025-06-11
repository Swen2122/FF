public class Inverter : Node
{
    public Inverter(Node child)
    {
        Attach(child);
    }

    public override NodeState Evaluate()
    {
        switch (children[0].Evaluate())
        {
            case NodeState.Running:
                state = NodeState.Running;
                break;
            case NodeState.Success:
                state = NodeState.Failure;
                break;
            case NodeState.Failure:
                state = NodeState.Success;
                break;
        }
        return state;
    }
}