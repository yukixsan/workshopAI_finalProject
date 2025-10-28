using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitAI : MonoBehaviour
{
    public float speed = 5f;
    public float nextWaypointDistance = 0.2f;

    Rigidbody rb;
    Vector3[] path;
    int targetIndex;

    IEnumerator  Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // keep upright
            yield return new WaitForSeconds(0.2f); // let the GridPath.Start() finish

            MoveTo(new Vector3(20, 0, 10));

    }

    public void MoveTo(Vector3 worldTarget)
    {
        PathRequestManager.RequestPath(transform.position, worldTarget, OnPathFound);
    }

    void OnPathFound(Vector3[] newPath, bool success)
    {
        if (success)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        else
        {
            path = null;
        }
    }

    IEnumerator FollowPath()
    {
        if (path == null || path.Length == 0) yield break;

        targetIndex = 0;
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            // Move toward waypoint (top-down: use x,z)
            Vector3 direction = (currentWaypoint - transform.position);
            direction.y = 0; // ignore vertical differences if any
            Vector3 velocity = direction.normalized * speed;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                 new Vector3(currentWaypoint.x, 0, currentWaypoint.z)) < nextWaypointDistance)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            yield return null;
        }
    }

    // optional: draw path for debugging
    void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.2f);
                if (i == targetIndex)
                    Gizmos.DrawLine(transform.position, path[i]);
                else
                    Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }
}
