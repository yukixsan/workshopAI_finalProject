using BehaviourTree;
using UnityEngine;
using System.Collections.Generic;

public class CooldownNode : Node
{
    private Node child;
    private float cooldownTime;
    private float lastUseTime = -Mathf.Infinity;

    public CooldownNode(Node child, float cooldownTime)
    {
        this.child = child;
        this.cooldownTime = cooldownTime;
    }

    public override NodeState Evaluate()
    {
         if (Time.time - lastUseTime < cooldownTime)
    {
        return NodeState.Failure; // instead of Running, fail so parent can retry later
    }

    var result = child.Evaluate();
    if (result == NodeState.Success)
    {
        lastUseTime = Time.time;
    }
    return result;
    }
}
