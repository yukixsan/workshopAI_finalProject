using UnityEngine;

public class BurstHorizontal : State
{
    [SerializeField] private ParticleSystem horizontalParticle; // Assign in Inspector
    [SerializeField] private ParticleSystem burstParticle;
    [SerializeField] private int minDuration; // Duration of the attack
    [SerializeField] private int maxDuration;

    protected BossTwoMachine stateMachine;
    private float timer;

    [SerializeField] private Vector3 movementAxis = Vector3.down; // Axis for movement (e.g., left/right = Vector3.right)
    [SerializeField] private float movementDistance = 4f; // Distance for back-and-forth movement
    [SerializeField] private float movementSpeed = 2f; // Speed of movement
    private Vector3 initialPosition; // To track the starting position
    [SerializeField] private bool movingForward = true; // Direction of movement
    [SerializeField] private bool isMoving = false;
    private void Awake()
    {
        stateMachine = GetComponent<BossTwoMachine>();
        initialPosition = transform.position;
    }
    public override void Enter()
    {
        Debug.Log("Entering Burst Horizontal State");

        isMoving = true;
        timer = Random.Range(minDuration, maxDuration);
        if (horizontalParticle != null)
        {
            horizontalParticle.gameObject.SetActive(true);
            horizontalParticle.Play(); // Start the particle effect
            burstParticle.gameObject.SetActive(true);
            burstParticle.Play();
        }
    }
    public override void Exit()
    {
        Debug.Log("Exiting Burst Horizontal State");

        isMoving = false;

        if (horizontalParticle != null)
        {
            horizontalParticle.Stop(); // Stop the particle effect
        }
        if(burstParticle != null)
        {
            burstParticle.Stop();
        }
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Transition to the next state (example: IdleState)
            stateMachine.ChangeState<SixthCirclingState>();
        }
        if (isMoving)
        {
            Move();
        }
    }
    private void Move()
    {
        float step = movementSpeed * Time.deltaTime;
        Vector3 targetPosition = movingForward
            ? initialPosition + movementAxis.normalized * movementDistance
            : initialPosition - movementAxis.normalized * movementDistance;

        // Move the enemy toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Check if the enemy reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            movingForward = !movingForward; // Reverse the direction
        }
    }
}
