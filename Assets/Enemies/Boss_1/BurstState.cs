using UnityEngine;

public class BurstState : BossAttackState
{
    

    

    public override void Enter()
    {
        Debug.Log("Entering Burst State");

        timer = Random.Range(minDuration, maxDuration);
        if (attackParticles != null)
        {
            attackParticles.gameObject.SetActive(true);
            attackParticles.Play(); // Start the particle effect
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Burst State");


        if (attackParticles != null)
        {
            attackParticles.Stop(); // Stop the particle effect
        }
    }

    public override void Update()
    {
        timer -= Time.deltaTime; // Decrease timer

        if (timer <= 0f)
        {
            // Transition to the next state (example: IdleState)
            stateMachine.ChangeState<SpreadState>();
        }
       
    }


}
