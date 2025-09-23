using BehaviourTree;
using UnityEngine;
using System.Collections.Generic;

public class PatrolNode : Node
{
    private Transform enemy;
    private Vector3 pointA, pointB;
    private float speed;
    private bool goingToB = true;

    public PatrolNode(Transform enemy, Vector3 offset, float speed)
    {
        this.enemy = enemy;
        this.speed = speed;
        pointA = enemy.position;
        pointB = enemy.position + offset;
    }

    public override NodeState Evaluate()
    {
        Vector3 target = goingToB ? pointB : pointA;
        enemy.position = Vector3.MoveTowards(enemy.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(enemy.position, target) < 0.1f)
            goingToB = !goingToB;

        return NodeState.Running;
    }
}
