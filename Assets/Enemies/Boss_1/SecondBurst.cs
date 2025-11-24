using UnityEngine;

public class SecondBurst : State
{
    [SerializeField] ParticleSystem _burstParticle;
    [SerializeField] private int duration;

    [Header("Waypoint Movement")]
    [SerializeField] private Transform[] waypoints;   // Assign in Inspector
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float stopDistance = 0.1f;

    private StateMachine stateMachine;
    private int waypointIndex = 0;
    private bool isMoving = false;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();    
    }

    public override void Enter()
    {
        print("Entering SecondBurst state");

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("SecondBurst: No waypoints assigned.");
            stateMachine.ChangeState<SecondRotate>();
            return;
        }

        waypointIndex = 0;
        isMoving = true;

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

        if (isMoving)
        {
            MoveAlongWaypoints();
        }
    }
     private void MoveAlongWaypoints()
    {
        Transform target = waypoints[waypointIndex];
        if (target == null) return;

        // Move toward current waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            movementSpeed * Time.deltaTime
        );

        // Check if reached the waypoint
        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            waypointIndex++;

            // Reached last waypoint → change state
            if (waypointIndex >= waypoints.Length)
            {
                stateMachine.ChangeState<SecondRotate>();
                return;
            }
        }
    }
}

