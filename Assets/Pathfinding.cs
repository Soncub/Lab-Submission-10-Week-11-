using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private List<Vector2Int> path = new List<Vector2Int>();
    public Vector2Int start = new Vector2Int(0, 1);
    public Vector2Int goal = new Vector2Int(4, 4);
    public Vector2Int[] presetObstacles;
    private Vector2Int next;
    private Vector2Int current;
    public int width = 5;
    public int height = 5;
    public float obstacleProbablity;

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    public int[,] grid = new int[5, 5];

    public void GenerateRandomGrid(int width, int height, float obstacleProbablity)
    {
        int obstacleTotal = (int)((width * height) * obstacleProbablity);
        int obstacleCount = 0;
        this.grid = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var number = 0;
                if (obstacleCount < obstacleTotal)
                {
                    number = Random.Range(0, 2);
                    if (number == 1)
                    {
                        obstacleCount++;
                    }
                }
                grid[i, j] = number;
            }
        }
    }

    public void AddObstacles(Vector2Int[] obstacleArray)
    {
        if (obstacleArray.Length > 0)
        {
            foreach (Vector2Int obstacle in obstacleArray)
            {
                grid[obstacle.y, obstacle.x] = 1;
            }

        }
        else
        {
            Debug.Log("No Obstacle Array");
        }       
    }

    private void Start()
    {
        GenerateRandomGrid(width, height, obstacleProbablity);
        AddObstacles(presetObstacles);
        FindPath(start, goal);
    }

    private void OnDrawGizmos()
    {
        //GenerateRandomGrid(width, height, obstacleProbablity);
        float cellSize = 1f;

        // Draw grid cells
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize, 0, y * cellSize);
                Gizmos.color = grid[y, x] == 1 ? Color.black : Color.white;
                Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
            }
        }

        // Draw path
        foreach (var step in path)
        {
            Vector3 cellPosition = new Vector3(step.x * cellSize, 0, step.y * cellSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
        }

        // Draw start and goal
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(start.x * cellSize, 0, start.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(goal.x * cellSize, 0, goal.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));
    }

    private bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.x < grid.GetLength(1) && point.y >= 0 && point.y < grid.GetLength(0);
    }

    private void FindPath(Vector2Int start, Vector2Int goal)
    {
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach (Vector2Int direction in directions)
            {
                next = current + direction;

                if (IsInBounds(next) && grid[next.y, next.x] == 0 && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.Log("Path not found.");
            return;
        }

        // Trace path from goal to start
        Vector2Int step = goal;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Add(start);
        path.Reverse();
    }
}
