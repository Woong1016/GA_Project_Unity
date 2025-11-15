using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;

public class MazeTest: MonoBehaviour
{
    public int width = 21;
    public int height = 21;

    public GameObject wall;
    public GameObject floor;
    public GameObject path;

    private int[,] map;
    private bool[,] visited;
    private Vector2Int goal;
    private Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) }; 

    private Transform mazeHolder;
    private Transform pathHolder;

    private List<Vector2Int> solutionPath;

    void Start()
    {
        GenerateAndDisplayMaze();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateAndDisplayMaze();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowPath();
        }
    }

    void GenerateAndDisplayMaze()
    {
        ClearMazeVisuals();

        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = 1;
            }
        }

        goal = new Vector2Int(width - 2, height - 2);

        CarvePath(1, 1);

        InstantiateMaze();

        solutionPath = new List<Vector2Int>();
        visited = new bool[map.GetLength(0), map.GetLength(1)];

        bool isSolvable = SearchMaze(1, 1, solutionPath);

       
    }

    void ClearMazeVisuals()
    {
        if (pathHolder != null)
        {
            Destroy(pathHolder.gameObject);
        }
        if (mazeHolder != null)
        {
            Destroy(mazeHolder.gameObject);
        }
        mazeHolder = new GameObject("MazeHolder").transform;
    }

    void InstantiateMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                if (map[x, y] == 1)
                {
                    Instantiate(wall, position, Quaternion.identity, mazeHolder);
                }
                else
                {
                    Instantiate(floor, position, Quaternion.identity, mazeHolder);
                }
            }
        }
    }

    void ShowPath()
    {
        if (pathHolder != null)
        {
            Destroy(pathHolder.gameObject);
        }
        pathHolder = new GameObject("PathHolder").transform;
        pathHolder.SetParent(mazeHolder);

        if (solutionPath == null || solutionPath.Count == 0)
        {
            
            return;
        }

        foreach (Vector2Int pos in solutionPath)
        {
            Vector3 position = new Vector3(pos.x, 0.1f, pos.y);
            Instantiate(path, position, Quaternion.identity, pathHolder);
        }
    }

    void CarvePath(int x, int y)
    {
        map[x, y] = 0;
        var shuffledDirs = dirs.OrderBy(d => Random.Range(0, 100)).ToArray();

        foreach (var d in shuffledDirs)
        {
            int nx = x + d.x * 2;
            int ny = y + d.y * 2;

            if (nx >= 1 && nx < width - 1 && ny >= 1 && ny < height - 1 && map[nx, ny] == 1)
            {
                map[x + d.x, y + d.y] = 0;
                CarvePath(nx, ny);
            }
        }
    }

    bool SearchMaze(int x, int y, List<Vector2Int> currentPath)
    {
        if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) return false;
        if (map[x, y] == 1 || visited[x, y]) return false;

        visited[x, y] = true;
        currentPath.Add(new Vector2Int(x, y));

        if (x == goal.x && y == goal.y) return true;

        foreach (var d in dirs)
        {
            if (SearchMaze(x + d.x, y + d.y, currentPath)) return true;
        }

        currentPath.RemoveAt(currentPath.Count - 1);
        return false;
    }
}