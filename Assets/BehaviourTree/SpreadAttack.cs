using BehaviourTree;
using UnityEngine;

public class SpreadAttack : Node
{
    private ParticleSystem spreadParticles;
    public SpreadAttack(ParticleSystem particles)
    {
        spreadParticles = particles;
    }

    public override NodeState Evaluate()
    {
        if (spreadParticles != null && !spreadParticles.isPlaying)
            spreadParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            spreadParticles.Play();

        Debug.Log("Spread Attack!");
        return NodeState.Success;
    }

}
