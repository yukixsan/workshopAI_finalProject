using UnityEngine;
using System.Collections.Generic;

public class SteeringAI : MonoBehaviour
{
    [Header("Wheels")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Wheel Meshes")]
    public Transform wheelFLMesh;
    public Transform wheelFRMesh;
    public Transform wheelRLMesh;
    public Transform wheelRRMesh;

    [Header("Patrol Points")]
    public List<Transform> waypoints;
    public float waypointThreshold = 3f;

    [Header("Car Settings")]
    public float maxMotorTorque = 1500f;
    public float maxSteerAngle = 30f;
    public float speed = 20f;

    private int currentWaypoint = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (waypoints.Count == 0) return;

        Vector3 target = waypoints[currentWaypoint].position;
        Vector3 localTarget = transform.InverseTransformPoint(target);

        // --- Steering ---
        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float steerAngle = steer * maxSteerAngle;
        wheelFL.steerAngle = steerAngle;
        wheelFR.steerAngle = steerAngle;

        // --- Motor ---
        float motor = maxMotorTorque;
        wheelRL.motorTorque = motor;
        wheelRR.motorTorque = motor;

        // --- Check waypoint reached ---
        if (Vector3.Distance(transform.position, target) < waypointThreshold)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        }

        // --- Update wheel meshes ---
        UpdateWheelVisuals(wheelFL, wheelFLMesh);
        UpdateWheelVisuals(wheelFR, wheelFRMesh);
        UpdateWheelVisuals(wheelRL, wheelRLMesh);
        UpdateWheelVisuals(wheelRR, wheelRRMesh);
    }

    void UpdateWheelVisuals(WheelCollider col, Transform mesh)
    {
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }
}
