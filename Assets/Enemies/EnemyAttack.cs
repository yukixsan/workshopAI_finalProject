using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class EnemyAttack : MonoBehaviour
{

    public ParticleSystem particle;
    public int damage  = 10;
    [SerializeField] PlayerHealth health;

    private void OnParticleTrigger()
    {
        print("collide");
        //Get particles collided
        List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();

        int enterCount = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);

        for (int i = 0; i < enterCount; i++) 
        { 
            health.TakeDamage(damage);
 
        }
    }

    private Collider GetColliderFromParticle(ParticleSystem.Particle particle)
    {
        return null;
    }
}
