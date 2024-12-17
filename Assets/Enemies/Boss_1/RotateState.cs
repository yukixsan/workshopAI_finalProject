using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotateState : State
{
    [SerializeField] private ParticleSystem rotateParticles; // Assign in Inspector
    [SerializeField] private float duration = 7f; // Duration of the attack

    protected StateMachine stateMachine;
    private float timer;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public override void Enter()
    {
        print("Start rotate attack_1");
        timer = duration; // Reset the timer
        if (rotateParticles != null)
        {
            rotateParticles.gameObject.SetActive(true);
            rotateParticles.Play(); // Start the particle effect
        }
    }

    public override void Exit()
    {
        print("Exiting spread attack_1");
        if(rotateParticles != null)
        {
            rotateParticles.Stop();
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

        //Debug.Log($"Rotate Attack Active: {timer:F2} seconds remaining");
    }
}
