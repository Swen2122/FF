using System.Collections.Generic;
public abstract class Node
{
    protected NodeState state;
    public Node parent;
    public List<Node> children = new List<Node>();
    private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

    public Node()
    {
        parent = null;
    }
    public virtual NodeState Evaluate() => NodeState.Failure;

    public void SetData(string key, object value)
    {
        _dataContext[key] = value;
    }

    public object GetData(string key)
    {
        if (_dataContext.TryGetValue(key, out object value))
            return value;

        Node node = parent;
        while (node != null)
        {
            if (node._dataContext.TryGetValue(key, out value))
                return value;
            node = node.parent;
        }
        return null;
    }

    public bool ClearData(string key)
    {
        if (_dataContext.ContainsKey(key))
        {
            _dataContext.Remove(key);
            return true;
        }

        Node node = parent;
        while (node != null)
        {
            if (node._dataContext.ContainsKey(key))
            {
                node._dataContext.Remove(key);
                return true;
            }
            node = node.parent;
        }
        return false;
    }

    public Node Attach(Node node)
    {
        node.parent = this;
        children.Add(node);
        return node;
    }

}
public enum NodeState
{
    Running,
    Success,
    Failure
}