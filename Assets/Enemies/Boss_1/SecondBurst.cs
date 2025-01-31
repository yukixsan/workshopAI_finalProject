using UnityEngine;

public class SecondBurst : State
{
    [SerializeField] ParticleSystem _burstParticle;
    [SerializeField] private int duration;

    protected StateMachine stateMachine;
    private float timer;

    [SerializeField] private Vector3 movementAxis = Vector3.up; // Axis for movement (e.g., left/right = Vector3.right)
    [SerializeField] private float movementDistance = 4f; // Distance for back-and-forth movement
    [SerializeField] private float movementSpeed = 2f; // Speed of movement
    private Vector3 initialPosition; // To track the starting position
    [SerializeField] private bool movingForward = true; // Direction of movement
    [SerializeField] private bool isMoving = false;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();    
        initialPosition = transform.position;
    }

    public override void Enter()
    {
        print("Entering second burst state");

        /*movementAxis = new Vector3(
           Random.Range(0, 2) == 0 ? 0 : 1, // Randomly -1 or 1 for X
           movementAxis.y,                   // Keep Y as is
           Random.Range(0, 2) == 0 ? 0 : 1);  // Randomly -1 or 1 for Z*/


        isMoving = true;
        timer = duration;
        if(_burstParticle != null)
        {
            _burstParticle.gameObject.SetActive(true);
            _burstParticle.Play();
        }
    }

    public override void Exit()
    {
        print("Exiting second burst state");

        isMoving = false;
        if (_burstParticle != null)
        {
            
            _burstParticle.Stop();
        }
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Transition to the next state (example: IdleState)
            stateMachine.ChangeState<SecondRotate>();
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

