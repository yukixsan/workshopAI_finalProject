using UnityEngine;

public class BossAttackState : State
{
    [Header("Shared Attack Settings")]
    [SerializeField] protected ParticleSystem attackParticles;
    [SerializeField] protected float minDuration = 3f;
    [SerializeField] protected float maxDuration = 5f;

    protected StateMachine stateMachine;
    protected float timer;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public override void Enter()
    {
        Debug.Log($"Entering {GetType().Name}");

        // Randomize duration
        timer = Random.Range(minDuration, maxDuration);

        // Start particle effect
        if (attackParticles != null)
        {
            attackParticles.gameObject.SetActive(true);
            attackParticles.Play();
        }
    }

    public override void Exit()
    {
        Debug.Log($"Exiting {GetType().Name}");

        if (attackParticles != null)
        {
            attackParticles.Stop();
            attackParticles.gameObject.SetActive(false);
        }
    }

    public override void Update()
    {
       
    }
}
