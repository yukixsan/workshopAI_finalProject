using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RotateState : BossAttackState
{
  

    [SerializeField] private Vector3 movementAxis = Vector3.up; // Axis for movement (e.g., left/right = Vector3.right)
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
        print("Start rotate attack_1");

        isMoving = true;
        timer = Random.Range(minDuration, maxDuration);
        if (attackParticles != null)
        {
            attackParticles.gameObject.SetActive(true);
            attackParticles.Play(); // Start the particle effect
        }
        attackParticles.transform.DORotate(new Vector3(0, 360, 0), timer , RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(1, LoopType.Incremental);
    }

    public override void Exit()
    {
        print("Exiting rotate attack_1");

        isMoving = false;
        if(attackParticles != null)
        {
            attackParticles.Stop();
        }
    }

    public override void Update()
    {
        

        timer -= Time.deltaTime; // Decrease timer

        if (timer <= 0f)
        {
            // Transition to the next state (example: IdleState)
            stateMachine.ChangeState<BurstState>();
        }

        if (isMoving)
        {
            Move();
        }

        //Debug.Log($"Rotate Attack Active: {timer:F2} seconds remaining");
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
