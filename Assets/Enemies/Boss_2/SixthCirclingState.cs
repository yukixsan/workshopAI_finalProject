using UnityEngine;

public class SixthCirclingState : State
{
    [SerializeField] private ParticleSystem circlingParticle; // Assign in Inspector
    [SerializeField] private int duration;

    protected BossTwoMachine stateMachine;
    private float timer;

    private void Awake()
    {
        stateMachine = GetComponent<BossTwoMachine>();
    }

    public override void Enter()
    {
        print("Entering circling 6th state");
        timer = duration;
        if (circlingParticle != null)
        {
            circlingParticle.gameObject.SetActive(true);
            circlingParticle.Play(); // Start the particle effect
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting 6th Circling State");


        if (circlingParticle != null)
        {
            circlingParticle.Stop(); // Stop the particle effect
        }
    }
    public override void Update()
    {
        int decideState = Random.Range(0, 1);

        timer -= Time.deltaTime; // Decrease timer

        if (timer <= 0f)
        {
            if(decideState == 0)             
            {
                stateMachine.ChangeState<BurstHorizontal>();
            }
            else
            {
                stateMachine.ChangeState<BurstVertical>();
            }
            
        }

    }
}
