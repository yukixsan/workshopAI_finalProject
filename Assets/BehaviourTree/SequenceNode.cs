using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;


public class SequenceNode : Node 
{
    private List<Node> children;

    public SequenceNode(List<Node> children) => this.children = children;

    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;
        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Failure:
                    state = NodeState.Failure;
                    return state;
                case NodeState.Running:
                    anyChildRunning = true;
                    break;
            }
        }
        state = anyChildRunning ? NodeState.Running : NodeState.Success;
        return state;
    }

    
}
