using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    GridPath grid;

    void Awake()
    {
        grid = GetComponent<GridPath>();
        if (grid == null) Debug.LogError("Pathfinding: Grid component missing on same GameObject.");
    }

    // Public entry point used by PathRequestManager
    public void StartFindPath(Vector3 startPos, Vector3 targetPos, System.Action<Vector3[], bool> callback)
    {
        StartCoroutine(FindPathCoroutine(startPos, targetPos, callback));
    }

    IEnumerator FindPathCoroutine(Vector3 startPos, Vector3 targetPos, System.Action<Vector3[], bool> callback)
    {
        NodePath startNode = grid.NodeFromWorldPoint(startPos);
        NodePath targetNode = grid.NodeFromWorldPoint(targetPos);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        if (startNode != null && targetNode != null && startNode.walkable && targetNode.walkable)
        {
            List<NodePath> openSet = new List<NodePath>();
            HashSet<NodePath> closedSet = new HashSet<NodePath>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                NodePath currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                        currentNode = openSet[i];
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (NodePath neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }

                // optional: yield to avoid frame spike if grid is big
                if (Time.frameCount % 100 == 0) yield return null;
            }
        }

        if (pathSuccess)
        {
            List<NodePath> path = RetracePath(startNode, targetNode);
            waypoints = SimplifyPath(path);
            pathSuccess = waypoints.Length > 0;
        }
        else
        {
            pathSuccess = false;
        }

        // callback to requester
        callback?.Invoke(waypoints, pathSuccess);

        yield return null;
    }

    List<NodePath> RetracePath(NodePath startNode, NodePath endNode)
    {
        List<NodePath> path = new List<NodePath>();
        NodePath currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    Vector3[] SimplifyPath(List<NodePath> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        if (path.Count == 0) return waypoints.ToArray();

        Vector2 oldDirection = Vector2.zero;
        for (int i = 0; i < path.Count; i++)
        {
            Vector2 newDir = Vector2.zero;
            if (i > 0)
            {
                newDir = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            }

            if (i == 0 || newDir != oldDirection)
            {
                waypoints.Add(path[i].worldPosition);
            }
            oldDirection = newDir;
        }
        return waypoints.ToArray();
    }

    int GetDistance(NodePath a, NodePath b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
