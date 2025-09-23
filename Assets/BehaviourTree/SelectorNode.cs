using BehaviourTree;
using UnityEngine;
using System.Collections.Generic;
public class SelectorNode : Node
{

    private List<Node> children;

    public SelectorNode(List<Node> children) => this.children = children;

    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Success:
                    state = NodeState.Success;
                    return state;
                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
            }
        }
        state = NodeState.Failure;
        return state;
    }
}
