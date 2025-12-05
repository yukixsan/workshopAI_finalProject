using UnityEngine;

public class CirclingState : State
{
    [SerializeField] private ParticleSystem circlingParticle; // Assign in Inspector
    [SerializeField] private int minDuration; // Duration of the attack
    [SerializeField] private int maxDuration;

    protected BossTwoMachine stateMachine;
    private float timer;

    private void Awake()
    {
        stateMachine = GetComponent<BossTwoMachine>();

    }

    public override void Enter()
    {
        Debug.Log("Entering Circling State");

        timer = Random.Range(minDuration, maxDuration);
        if (circlingParticle != null)
        {
            circlingParticle.gameObject.SetActive(true);
            circlingParticle.Play(); // Start the particle effect
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Circling State");


        if (circlingParticle != null)
        {
            circlingParticle.Stop(); // Stop the particle effect
        }
    }

    public override void Update()
    {
        timer -= Time.deltaTime; // Decrease timer

        if (timer <= 0f)
        {
            // Transition to the next state (example: IdleState)
            stateMachine.ChangeState<VerticalState>();
        }

    }

}
