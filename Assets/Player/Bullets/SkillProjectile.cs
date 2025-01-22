using System.Collections.Generic;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    public ParticleSystem particle;
    public int damage;

    private void OnParticleCollision(GameObject other)
    {
        // Check if the collided object has the EnemyHealth component
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            // Apply damage to the enemy
            enemyHealth.TakeDamage(damage);
            Debug.Log($"Particle dealt {damage} damage to {other.name}");
        }
        else
        {
            Debug.Log($"Particle collided with {other.name}, but it has no EnemyHealth component.");
        }
    }
}
