using BehaviourTree;
using UnityEngine;

public class IsPlayerInRange : Node
{
    private Transform enemy, player;
    private float range;

    public IsPlayerInRange(Transform enemy, Transform player, float range)
    {
        this.enemy = enemy;
        this.player = player;
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        float dist = Vector3.Distance(enemy.position, player.position);
        return dist <= range ? NodeState.Success : NodeState.Failure;
    }
}


