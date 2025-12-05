using UnityEngine;

public class GridPath : MonoBehaviour
{


    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    NodePath[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new NodePath[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position 
            - Vector3.right * gridWorldSize.x / 2 
            - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft 
                    + Vector3.right * (x * nodeDiameter + nodeRadius) 
                    + Vector3.forward * (y * nodeDiameter + nodeRadius);

            Vector3 checkPos = worldPoint + Vector3.up * 0.2f; // small height offset
                bool walkable = !Physics.CheckSphere(checkPos, nodeRadius, unwalkableMask);
                grid[x, y] = new NodePath(walkable, worldPoint, x, y);
               
            }
        }
    }

    public NodePath NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public System.Collections.Generic.List<NodePath> GetNeighbours(NodePath node)
    {
        var neighbours = new System.Collections.Generic.List<NodePath>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }

    void OnDrawGizmos()
{
    Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

    if (grid != null)
    {
        foreach (NodePath n in grid)
        {
            Gizmos.color = n.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            if (!n.walkable)
    Debug.DrawRay(n.worldPosition, Vector3.up * 2, Color.red, 2f);
else
    Debug.DrawRay(n.worldPosition, Vector3.up * 2, Color.green, 2f);
        }
    }
    else
    {
        // Generate a preview grid in the editor
        nodeDiameter = nodeRadius * 2;
        int gridX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        int gridY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                Gizmos.color = Color.gray;
                    Gizmos.DrawCube(worldPoint, Vector3.one * (nodeDiameter - 0.1f));
                 
            }
        }
    }
}

}


