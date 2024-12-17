using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class SpreadState : State
{
    [SerializeField] private ParticleSystem spreadParticles; // Assign in Inspector
    [SerializeField] private float duration = 5f; // Duration of the attack

    private StateMachine stateMachine;
    private float timer;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public override void Enter()
    {
        Debug.Log("Entering Spread State");

        timer = duration; // Reset the timer
        if (spreadParticles != null)
        {
            spreadParticles.gameObject.SetActive(true);
            spreadParticles.Play(); // Start the particle effect
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Spread State");

        if (spreadParticles != null)
        {
            spreadParticles.Stop(); // Stop the particle effect
        }
    }

    public override void Update()
    {
        timer -= Time.deltaTime; // Decrease timer

        if (timer <= 0f)
        {
            // Transition to the next state (example: IdleState)
            stateMachine.ChangeState<RotateState>();
        }

        //Debug.Log($"Spread Attack Active: {timer:F2} seconds remaining");
    }
}
