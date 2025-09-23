using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class SpreadState : BossAttackState
{

 

    [SerializeField] private Vector3 movementAxis = Vector3.right; // Axis for movement (e.g., left/right = Vector3.right)
    [SerializeField] private float movementDistance = 4f; // Distance for back-and-forth movement
    [SerializeField] private float movementSpeed = 2f; // Speed of movement
    private Vector3 initialPosition; // To track the starting position
    [SerializeField] private bool movingForward = true; // Direction of movement
    [SerializeField] private bool isMoving = false;

    protected override void Awake()
    {
        base.Awake();
                initialPosition = transform.position;

    }

    public override void Enter()
    {
        Debug.Log("Entering Spread State");

        isMoving = true;
        timer = Random.Range(minDuration, maxDuration);
        if (attackParticles != null)
        {
            attackParticles.gameObject.SetActive(true);
            attackParticles.Play(); // Start the particle effect
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Spread State");

        isMoving = false;

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
            stateMachine.ChangeState<RotateState>();
        }
        if(isMoving)
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

