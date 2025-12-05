using BehaviourTree;
using UnityEngine;

public class RotateAttack : Node
{
    private ParticleSystem rotateAttack;
    public RotateAttack(ParticleSystem particles)
    {
        rotateAttack = particles;
    }

    public override NodeState Evaluate()
    {
        if (rotateAttack != null && !rotateAttack.isPlaying)
            rotateAttack.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            rotateAttack.Play();

        Debug.Log("Rotate Attack!");
        return NodeState.Success;
    }
}
