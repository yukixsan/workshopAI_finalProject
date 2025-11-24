using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private  float moveSpeed = 3f;
    [SerializeField] private  float stopDistance = 1f;

    void Update()
    {
        if (player == null) return;

        // Distance to player
        float dist = Vector3.Distance(transform.position, player.position);

        // Stop if close enough
        if (dist <= stopDistance)
            return;

        // Direction to player
        Vector3 dir = (player.position - transform.position).normalized;

        // Do NOT modify rotation; just move
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
