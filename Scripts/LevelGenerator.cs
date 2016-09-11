using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour
{
    public int amountOfNodesInARow;
    public float gapBetweenNodes;
    public GameObject nodePrefab;
    static GameObject[,] nodes;
    GameObject parentOfNodes;

    List<intVector2> waypoints;
    Cell current;
    Cell[,] grid;
    Stack<Cell> stack;
    List<intVector2> roadNodes;

    public GameObject roadPrefab;
    GameObject roadsParent;

    public GameObject towerPrefab;
    public GameObject spawnPrefab;

    public static Vector3 spawnPoint;
    public static List<Vector3> targets;
    public static int maxNodesInARow;

    void Start()
    {
        Time.timeScale = 1;

        nodes = new GameObject[amountOfNodesInARow, amountOfNodesInARow];
        parentOfNodes = new GameObject("Nodes");
        GenerateNodes();
        GenerateStartNEnd();

        grid = new Cell[amountOfNodesInARow, amountOfNodesInARow];
        stack = new Stack<Cell>();
        FillGrid();
        current = grid[1, 1];
        current.visited = true;
        CreatePath();
        TranslatePathToWaypoints();

        roadsParent = new GameObject("Roads");
        CreateRoads();
        DeleteRoadNodes();

        spawnPoint = nodes[1, 1].transform.position;
        maxNodesInARow = amountOfNodesInARow;
        FillTargets();
    }

    void GenerateNodes()
    {
        for (int x = 0; x < amountOfNodesInARow; x++)
        {
            for (int y = 0; y < amountOfNodesInARow; y++)
            {
                Vector3 position;

                if (x == 0 && y == 0)
                {
                    float firstNodePos = (gapBetweenNodes + nodePrefab.transform.localScale.x) / 2f * (amountOfNodesInARow - 1f);
                    position = new Vector3(-firstNodePos, 0, -firstNodePos);
                }
                else if (y == 0)
                {
                    float newPos = nodes[x - 1, y].transform.position.x + nodePrefab.transform.localScale.x + gapBetweenNodes;
                    position = new Vector3(newPos, 0, nodes[x - 1, y].transform.position.z);
                }
                else
                {
                    float newPos = nodes[x, y - 1].transform.position.z + nodePrefab.transform.localScale.x + gapBetweenNodes;
                    position = new Vector3(nodes[x, y - 1].transform.position.x, 0, newPos);
                }

                nodes[x, y] = (GameObject)Instantiate(nodePrefab, position, Quaternion.identity);

                nodes[x, y].transform.parent = parentOfNodes.transform;
            }
        }
    }

    void GenerateStartNEnd()
    {
        Instantiate(spawnPrefab, nodes[1, 1].transform.position + (new Vector3(0, nodes[0, 0].transform.localScale.y) + new Vector3(0, spawnPrefab.transform.localScale.y)) / 2f, Quaternion.identity);
        Instantiate(towerPrefab, nodes[amountOfNodesInARow - 2, amountOfNodesInARow - 2].transform.position + (new Vector3(0, nodes[0, 0].transform.localScale.y) + new Vector3(0, towerPrefab.transform.localScale.y)) / 2f, Quaternion.identity);
    }

    void FillGrid()
    {
        for (int x = 0; x < amountOfNodesInARow; x++)
        {
            for (int y = 0; y < amountOfNodesInARow; y++)
            {
                grid[x, y] = new Cell(x, y);

                if (x == 0 || y == 0 || x == amountOfNodesInARow - 1 || y == amountOfNodesInARow - 1)
                    grid[x, y].visited = true;
            }
        }
    }

    void CreatePath()
    {
        Cell end = new Cell(amountOfNodesInARow - 2, amountOfNodesInARow - 2);

        while (current.x != end.x || current.y != end.y) // While haven't reached the end
        {
            Cell next = current.GetNeighbor(grid); // Choose randomly one of the unvisited neighbours
            if (next != null) // If the current cell has any neighbours which have not been visited
            {
                stack.Push(current); // Push the current cell to the stack
                // Make the chosen cell the current cell and mark it as visited
                for (int i = 0; i < amountOfNodesInARow; i++)
                {
                    bool found = false;
                    for (int j = 0; j < amountOfNodesInARow; j++)
                    {
                        if (grid[i, j] == next)
                        {
                            found = true;
                            current = grid[i, j];
                            break;
                        }
                    }
                    if (found)
                        break;
                }
                current.visited = true;
            }
            else if (stack.Count != 0) // Else if stack is not empty
            {
                current = stack.Pop();
            }
        }

        stack.Push(end);
    }

    void TranslatePathToWaypoints()
    {
        List<intVector2> waypointsReverse = new List<intVector2>();
        waypoints = new List<intVector2>();
        roadNodes = new List<intVector2>();

        while (stack.Count > 0)
        {
            Cell newCell = stack.Pop();
            intVector2 newPos = new intVector2(newCell.x, newCell.y);
            waypointsReverse.Add(newPos);
        }

        for (int i = waypointsReverse.Count - 1; i >= 0; i--)
        {
            waypoints.Add(waypointsReverse[i]);
            roadNodes.Add(waypointsReverse[i]);
        }
        CleanUpWaypoints();
    }

    void CleanUpWaypoints()
    {
        for (int i = 1; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i - 1].x != waypoints[i + 1].x && waypoints[i - 1].y != waypoints[i + 1].y)
                continue;

            waypoints.RemoveAt(i);
        }
    }

    void CreateRoads()
    {
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 roadPos = new Vector3();
            Vector3 roadScale = new Vector3();
            float offsetScale = roadPrefab.transform.localScale.x;

            if (waypoints[i].x == waypoints[i + 1].x)
            {
                float offsetScaleY = (nodes[waypoints[i].x, waypoints[i].y].transform.position.z - nodes[waypoints[i + 1].x, waypoints[i + 1].y].transform.position.z < 0) ? -offsetScale : offsetScale;
                roadPos = new Vector3(nodes[waypoints[i].x, waypoints[i].y].transform.position.x, 0, (nodes[waypoints[i].x, waypoints[i].y].transform.position.z + nodes[waypoints[i + 1].x, waypoints[i + 1].y].transform.position.z) / 2f);
                roadScale = new Vector3(offsetScale, roadPrefab.transform.localScale.y, Mathf.Abs(nodes[waypoints[i].x, waypoints[i].y].transform.position.z - nodes[waypoints[i + 1].x, waypoints[i + 1].y].transform.position.z + offsetScaleY));
            }
            else if (waypoints[i].y == waypoints[i + 1].y)
            {
                float offsetScaleX = (nodes[waypoints[i].x, waypoints[i].y].transform.position.x - nodes[waypoints[i + 1].x, waypoints[i + 1].y].transform.position.x < 0) ? -offsetScale : offsetScale;
                roadPos = new Vector3((nodes[waypoints[i].x, waypoints[i].y].transform.position.x + nodes[waypoints[i + 1].x, waypoints[i + 1].y].transform.position.x) / 2f, 0, nodes[waypoints[i].x, waypoints[i].y].transform.position.z);
                roadScale = new Vector3(Mathf.Abs(nodes[waypoints[i].x, waypoints[i].y].transform.position.x - nodes[waypoints[i + 1].x, waypoints[i + 1].y].transform.position.x + offsetScaleX), roadPrefab.transform.localScale.y, offsetScale);
            }

            GameObject road = (GameObject)Instantiate(roadPrefab, roadPos, Quaternion.identity, roadsParent.transform);
            road.transform.localScale = roadScale;
        }
    }

    void DeleteRoadNodes()
    {
        for (int i = 0; i < roadNodes.Count; i++)
        {
            Destroy(nodes[roadNodes[i].x, roadNodes[i].y]);
        }
    }

    void FillTargets()
    {
        targets = new List<Vector3>();
        for(int i = 1; i < waypoints.Count; i++)
        {
            targets.Add(new Vector3(nodes[waypoints[i].x, waypoints[i].y].transform.position.x, 0.4f, nodes[waypoints[i].x, waypoints[i].y].transform.position.z));
        }
    }
}

class Cell
{
    public int x;
    public int y;
    public bool visited;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        visited = false;
    }

    public Cell(int x, int y, bool visited)
    {
        this.x = x;
        this.y = y;
        this.visited = visited;
    }

    public Cell GetNeighbor(Cell[,] grid)
    {
        List<Cell> neighbors = new List<Cell>();

        int amountOfNodesInARow = grid.GetLength(0);

        Cell top = grid[x, y + 1];
        Cell right = grid[x + 1, y];
        Cell bottom = grid[x, y - 1];
        Cell left = grid[x - 1, y];

        Cell end = new Cell(amountOfNodesInARow - 2, amountOfNodesInARow - 2);

        if (top != null && !top.visited)
        {
            neighbors.Add(top);
        }
        if (right != null && !right.visited)
        {
            neighbors.Add(right);
        }
        if (bottom != null && !bottom.visited)
        {
            neighbors.Add(bottom);
        }
        if (left != null && !left.visited)
        {
            neighbors.Add(left);
        }

        if (neighbors.Count > 0) // If there's any neighbors
        {
            for (int i = 0; i < neighbors.Count; i++) // Return end if can go there
            {
                if (neighbors[i].x == end.x && neighbors[i].y == end.y)
                    return neighbors[i];
            }

            int random = UnityEngine.Random.Range(0, neighbors.Count);
            return neighbors[random];
        }
        return null;
    }
}

class intVector2 : IEquatable<intVector2>
{
    public int x;
    public int y;

    public intVector2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public bool Equals(intVector2 other)
    {
        if (other == null)
            return false;

        return other.x == x && other.y == y;
    }
}