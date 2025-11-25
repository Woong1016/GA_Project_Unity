using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class BFSTest : MonoBehaviour
{
    public int width = 21;
    public int height = 21;

    public GameObject wall;
    public GameObject floor;
    public GameObject path;

    public GameObject CharacterPrefab;

    private int[,] map;
    private bool[,] visited;
    private Vector2Int goal;
    private Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

    private Transform mazeHolder;
    private Transform pathHolder;

    private List<Vector2Int> solutionPath;

    private Vector2Int?[,] parent; 

    void Start()
    {
        GenerateAndDisplayMaze();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowPath();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            List<Vector2Int> path = FindPathBFS();
            if (path != null)
            {
                StartCoroutine(Charactermove(path));
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            GenerateAndDisplayMaze();
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

        //List < Vector2Int > pathList= new List<Vector2Int>();
        List<Vector2Int> pathList = FindPathBFS();
        if(pathList == null)
        {
            return;
        }

       //if (solutionPath == null || solutionPath.Count == 0)
       //{
       //
       //    return;
       //}

        foreach (Vector2Int pos in pathList)
        {
            Vector3 position = new Vector3(pos.x, 0.1f, pos.y);
            Instantiate(path, position, Quaternion.identity, pathHolder);
        }
    }

    IEnumerator Charactermove(List<Vector2Int>pathList)
    {
        if(pathList == null|| pathList.Count == 0) yield break;

        Vector2Int StartPos = pathList[0];
        //GameObject 여기까지 만듬 2025. 11.19 이어서 만들자 
        GameObject unit = Instantiate(CharacterPrefab, new Vector3(StartPos.x, 0.5f, StartPos.y), Quaternion.identity);

        foreach(Vector2Int target in pathList)
        {
            Vector3 Startpos = unit.transform.position;
            Vector3 EndPos = new Vector3(target.x, 0.5f, target.y);
            float time = 0f;
            float duration = 0.2f;

            while (time < duration)
            {
                unit.transform.position = Vector3.Lerp(Startpos, EndPos, time / duration);
                time += Time.deltaTime;
                yield return null;

            }
            unit.transform.position = EndPos;
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
    List<Vector2Int> FindPathBFS()
    {
        int w = map.GetLength(0); // x 크기//
        int h = map.GetLength(1); // y 크기//
        bool[,] visited = new bool[w, h];
        parent = new Vector2Int?[w, h];
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        Vector2Int startPos = new Vector2Int(1,1);
       q.Enqueue(startPos);
       visited[startPos.x, startPos.y] = true;

        while (q.Count > 0)
        {
            Vector2Int cur = q.Dequeue();

            // 목표 도착
            if (cur == goal)
            {
                Debug.Log("최단경로 생성완");
                return ReconstructPath();
            }

            // 네 방향 이웃 탐색
            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (!InBounds(nx, ny)) continue; // 전체 바운더리
                if (map[nx, ny] == 1) continue;  // 벽
                if (visited[nx, ny]) continue;   // 이미 방문

                visited[nx, ny] = true;
                parent[nx, ny] = cur;            // 경로 복원용 부모
                q.Enqueue(new Vector2Int(nx, ny));
            }
        }

        Debug.Log("BFS: 경로 없음");
        return null;
    }

    bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 &&
               x < map.GetLength(0) &&
               y < map.GetLength(1);
    }

    List<Vector2Int> ReconstructPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? cur = goal;

        // goal -> start 방향으로 parent 따라가기
        while (cur.HasValue)
        {
            path.Add(cur.Value);
            cur = parent[cur.Value.x, cur.Value.y];
        }

        path.Reverse(); // start -> goal 순서로 반전
        Debug.Log($"경로 길이: {path.Count}");
        foreach (var p in path)
        {
            Debug.Log(p);
        }
        return path;
    }
}