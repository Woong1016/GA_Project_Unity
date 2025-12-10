using System.Collections.Generic;
using UnityEngine;

public class AStar2Study : MonoBehaviour
{
    public int width = 21;
    public int height = 21;
    [Range(0, 50)] public int wallPercent = 20;
    public int enemyCount = 5;

    int[,] map;
    Vector2Int startPos;
    Vector2Int goalPos;
    List<Vector2Int> enemies = new List<Vector2Int>();

    Transform mapContainer;
    Transform pathContainer;

    void Start()
    {
        do { GenerateMap(); } while (!IsSolvable());
        SpawnEnemies();
        DrawMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var path = AStar(map, startPos, goalPos);
            DrawPath(path);
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1) { map[x, y] = 0; continue; }

                int rand = Random.Range(0, 100);
                if (rand < wallPercent) map[x, y] = 0;
                else if (rand < wallPercent + 50) map[x, y] = 1;
                else if (rand < wallPercent + 80) map[x, y] = 2;
                else map[x, y] = 3;
            }
        }
        startPos = new Vector2Int(1, 1);
        goalPos = new Vector2Int(width - 2, height - 2);
        map[startPos.x, startPos.y] = 1;
        map[goalPos.x, goalPos.y] = 1;
    }

    void SpawnEnemies()
    {
        enemies.Clear();
        int spawned = 0;
        while (spawned < enemyCount)
        {
            int x = Random.Range(1, width - 1);
            int y = Random.Range(1, height - 1);

            if (map[x, y] != 0 && new Vector2Int(x, y) != startPos && new Vector2Int(x, y) != goalPos && !enemies.Contains(new Vector2Int(x, y)))
            {
                enemies.Add(new Vector2Int(x, y));
                spawned++;
            }
        }
    }

    bool IsSolvable()
    {
        bool[,] visited = new bool[width, height];
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(startPos);
        visited[startPos.x, startPos.y] = true;
        int[] dx = { 0, 0, 1, -1 }; int[] dy = { 1, -1, 0, 0 };

        while (q.Count > 0)
        {
            Vector2Int cur = q.Dequeue();
            if (cur == goalPos) return true;
            for (int i = 0; i < 4; i++)
            {
                int nx = cur.x + dx[i]; int ny = cur.y + dy[i];
                if (nx < 0 || ny < 0 || nx >= width || ny >= height || map[nx, ny] == 0 || visited[nx, ny]) continue;
                visited[nx, ny] = true;
                q.Enqueue(new Vector2Int(nx, ny));
            }
        }
        return false;
    }

    List<Vector2Int> AStar(int[,] map, Vector2Int start, Vector2Int goal)
    {
        int w = map.GetLength(0); int h = map.GetLength(1);
        int[,] gCost = new int[w, h];
        for (int x = 0; x < w; x++) for (int y = 0; y < h; y++) gCost[x, y] = int.MaxValue;
        gCost[start.x, start.y] = 0;

        List<Vector2Int> open = new List<Vector2Int> { start };
        Vector2Int?[,] parent = new Vector2Int?[w, h];
        bool[,] visited = new bool[w, h];
        Vector2Int[] dirs = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

        while (open.Count > 0)
        {
            int bestIdx = 0; int bestF = int.MaxValue;
            for (int i = 0; i < open.Count; i++)
            {
                int f = gCost[open[i].x, open[i].y] + GetHeuristic(open[i], goal);
                if (f < bestF) { bestF = f; bestIdx = i; }
            }
            Vector2Int cur = open[bestIdx]; open.RemoveAt(bestIdx);

            if (cur == goal) return Reconstruct(parent, start, goal);
            visited[cur.x, cur.y] = true;

            foreach (var d in dirs)
            {
                int nx = cur.x + d.x; int ny = cur.y + d.y;
                if (nx < 0 || ny < 0 || nx >= w || ny >= h || map[nx, ny] == 0 || visited[nx, ny]) continue;

                int newG = gCost[cur.x, cur.y] + GetTileCost(map[nx, ny]);
                if (newG < gCost[nx, ny])
                {
                    gCost[nx, ny] = newG;
                    parent[nx, ny] = cur;
                    if (!open.Contains(new Vector2Int(nx, ny))) open.Add(new Vector2Int(nx, ny));
                }
            }
        }
        return null;
    }

    int GetHeuristic(Vector2Int a, Vector2Int b)
    {
        int h = Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        foreach (var enemy in enemies)
        {
            float dist = Vector2Int.Distance(a, enemy);

            if (dist < 5.0f)
            {
                h += (int)(20.0f / (dist + 0.1f));
            }
        }

        return h;
    }

    int GetTileCost(int type)
    {
        switch (type) { case 1: return 1; case 2: return 3; case 3: return 5; default: return 999; }
    }

    List<Vector2Int> Reconstruct(Vector2Int?[,] parent, Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? cur = goal;
        while (cur.HasValue) { path.Add(cur.Value); if (cur.Value == start) break; cur = parent[cur.Value.x, cur.Value.y]; }
        path.Reverse();
        return path;
    }

    void DrawMap()
    {
        if (mapContainer != null) Destroy(mapContainer.gameObject);
        mapContainer = new GameObject("Map Container").transform;
        mapContainer.parent = transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x, 0, y);
                cube.transform.parent = mapContainer;

                Renderer rend = cube.GetComponent<Renderer>();
                if (map[x, y] == 0) rend.material.color = Color.black;
                else if (map[x, y] == 1) rend.material.color = Color.white;
                else if (map[x, y] == 2) rend.material.color = Color.green;
                else if (map[x, y] == 3) rend.material.color = new Color(0.6f, 0.4f, 0.2f);
            }
        }

        foreach (var enemy in enemies)
        {
            GameObject enemyCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            enemyCube.transform.position = new Vector3(enemy.x, 1f, enemy.y);
            enemyCube.transform.localScale = Vector3.one * 0.8f;
            enemyCube.transform.parent = mapContainer;
            enemyCube.GetComponent<Renderer>().material.color = Color.red;
            enemyCube.name = "Enemy";
        }
    }

    void DrawPath(List<Vector2Int> path)
    {
        if (path == null) return;
        if (pathContainer != null) Destroy(pathContainer.gameObject);
        pathContainer = new GameObject("Path Container").transform;
        pathContainer.parent = transform;

        foreach (var p in path)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(p.x, 0.5f, p.y);
            cube.transform.localScale = Vector3.one * 0.5f;
            cube.transform.parent = pathContainer;
            cube.GetComponent<Renderer>().material.color = Color.cyan;
        }
    }
}