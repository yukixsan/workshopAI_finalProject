using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class EnemyAI: MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;

    [Header("AI Settings")]
    public Transform target;
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float chaseRange = 8f;
    public float steeringForce = 8f;
    public float maxSpeed = 3.5f;

    private int patrolIndex;
    private float waitTimer;
    private NavMeshPath path;
    private int pathIndex;

    void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        path = new NavMeshPath();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < chaseRange)
            agent.CalculatePath(target.position, path);
        else
            PatrolPathLogic();
    }

    void FixedUpdate()
    {
        FollowPathWithSteering();
    }

    // ----------- PATROL -----------
    void PatrolPathLogic()
    {
        if (patrolPoints.Length == 0) return;

        if (ReachedPathEnd())
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                GoToNextPatrolPoint();
                waitTimer = 0f;
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        agent.CalculatePath(patrolPoints[patrolIndex].position, path);
        pathIndex = 0;
    }

    // ----------- MOVEMENT / STEERING -----------
    void FollowPathWithSteering()
    {
        if (path == null || path.corners.Length == 0 || pathIndex >= path.corners.Length) return;

        Vector3 targetPos = path.corners[pathIndex];
        Vector3 toTarget = targetPos - transform.position;
        toTarget.y = 0f;

        if (toTarget.magnitude < 0.4f)
        {
            pathIndex++;
            return;
        }

        Vector3 desiredVelocity = toTarget.normalized * maxSpeed;
        Vector3 steering = desiredVelocity - rb.linearVelocity;

        rb.AddForce(steering * steeringForce, ForceMode.Acceleration);

        // Clamp velocity
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        // Rotate smoothly
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(rb.linearVelocity.normalized);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.deltaTime * 5f));
        }

        agent.nextPosition = transform.position;
    }

    bool ReachedPathEnd()
    {
        return pathIndex >= path.corners.Length;
    }
}
