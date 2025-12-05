using BehaviourTree;
using UnityEngine;
using System.Collections.Generic;

public class RandomSelector : Node
{
    private List<Node> children;
    private System.Random rng = new System.Random();

    public RandomSelector(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        if (children.Count == 0) return NodeState.Failure;

        int index = rng.Next(children.Count);
        return children[index].Evaluate();
    }
}
