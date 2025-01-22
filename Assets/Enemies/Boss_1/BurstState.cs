using UnityEngine;

public class BurstState : State
{
    [SerializeField] private ParticleSystem burstParticles; // Assign in Inspector
    [SerializeField] private int minDuration; // Duration of the attack
    [SerializeField] private int maxDuration;

    protected StateMachine stateMachine;
    private float timer;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
      
    }

    public override void Enter()
    {
        Debug.Log("Entering Burst State");

        timer = Random.Range(minDuration, maxDuration);
        if (burstParticles != null)
        {
            burstParticles.gameObject.SetActive(true);
            burstParticles.Play(); // Start the particle effect
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Burst State");


        if (burstParticles != null)
        {
            burstParticles.Stop(); // Stop the particle effect
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
