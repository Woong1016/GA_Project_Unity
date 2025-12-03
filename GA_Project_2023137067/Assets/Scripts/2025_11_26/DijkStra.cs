using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DijkStra : MonoBehaviour
{
    public int w = 21;
    public int h = 21;

    public GameObject wallObj;
    public GameObject floorObj;
    public GameObject treeObj;
    public GameObject mudObj;
    public GameObject pathObj;

    int[,] map;

    List<GameObject> currentMapObjs = new List<GameObject>();
    List<GameObject> pathObjs = new List<GameObject>();

    Vector2Int startPoint = new Vector2Int(1, 1);
    Vector2Int endPoint;

    public class SimplePriorityQueue
    {
        List<Node> list = new List<Node>();

        public void Push(Node node)
        {
            list.Add(node);
            list.Sort((a, b) => a.cost.CompareTo(b.cost));
        }

        public Node Pop()
        {
            if (list.Count == 0) return null;
            Node n = list[0];
            list.RemoveAt(0);
            return n;
        }

        public int Count { get { return list.Count; } }
    }

    public class Node
    {
        public int x, y;
        public int cost;
        public Node parent;

        public Node(int x, int y, int cost, Node parent)
        {
            this.x = x;
            this.y = y;
            this.cost = cost;
            this.parent = parent;
        }
    }

    void Start()
    {
        if (w % 2 == 0) w++;
        if (h % 2 == 0) h++;

        endPoint = new Vector2Int(w - 2, h - 2);

        CreateValidMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FindPath();
        }
    }

    void FindPath()
    {
        for (int i = 0; i < pathObjs.Count; i++)
        {
            Destroy(pathObjs[i]);
        }
        pathObjs.Clear();

        List<Vector2Int> path = RunDijkstra();

        if (path != null)
        {
            Debug.Log("Path Found: " + path.Count);
            StartCoroutine(ShowPath(path));
        }
        else
        {
            Debug.Log("Path Not Found");
        }
    }

    void CreateValidMap()
    {
        while (true)
        {
            MakeRandomMaze();

            if (CheckPathDFS())
            {
                DrawMap();
                break;
            }
        }
    }

    void MakeRandomMaze()
    {
        map = new int[w, h];

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                map[x, y] = 0;

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(1, 1);
        map[current.x, current.y] = 1;
        stack.Push(current);

        int[] dx = { 0, 0, 2, -2 };
        int[] dy = { 2, -2, 0, 0 };

        while (stack.Count > 0)
        {
            current = stack.Pop();

            List<int> dirList = new List<int>();
            for (int k = 0; k < 4; k++) dirList.Add(k);

            for (int k = 0; k < dirList.Count; k++)
            {
                int r = Random.Range(0, dirList.Count);
                int temp = dirList[k];
                dirList[k] = dirList[r];
                dirList[r] = temp;
            }

            foreach (int i in dirList)
            {
                int nx = current.x + dx[i];
                int ny = current.y + dy[i];

                if (nx > 0 && nx < w - 1 && ny > 0 && ny < h - 1)
                {
                    if (map[nx, ny] == 0)
                    {
                        stack.Push(current);

                        map[current.x + dx[i] / 2, current.y + dy[i] / 2] = 1;
                        map[nx, ny] = 1;

                        stack.Push(new Vector2Int(nx, ny));
                        break;
                    }
                }
            }
        }

        for (int x = 1; x < w - 1; x++)
        {
            for (int y = 1; y < h - 1; y++)
            {
                if (map[x, y] == 1)
                {
                    int r = Random.Range(0, 100);
                    if (r < 15) map[x, y] = 2;
                    else if (r < 25) map[x, y] = 3;
                }
            }
        }

        map[startPoint.x, startPoint.y] = 1;
        map[endPoint.x, endPoint.y] = 1;
    }

    bool CheckPathDFS()
    {
        bool[,] visited = new bool[w, h];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        stack.Push(startPoint);
        visited[startPoint.x, startPoint.y] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (stack.Count > 0)
        {
            Vector2Int cur = stack.Pop();

            if (cur == endPoint) return true;

            for (int i = 0; i < 4; i++)
            {
                int nx = cur.x + dx[i];
                int ny = cur.y + dy[i];

                if (nx >= 0 && nx < w && ny >= 0 && ny < h)
                {
                    if (map[nx, ny] != 0 && visited[nx, ny] == false)
                    {
                        visited[nx, ny] = true;
                        stack.Push(new Vector2Int(nx, ny));
                    }
                }
            }
        }
        return false;
    }

    List<Vector2Int> RunDijkstra()
    {
        SimplePriorityQueue pq = new SimplePriorityQueue();

        int[,] dist = new int[w, h];
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                dist[x, y] = 999999;

        dist[startPoint.x, startPoint.y] = 0;

        pq.Push(new Node(startPoint.x, startPoint.y, 0, null));

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        Node targetNode = null;

        while (pq.Count > 0)
        {
            Node cur = pq.Pop();

            if (cur.x == endPoint.x && cur.y == endPoint.y)
            {
                targetNode = cur;
                break;
            }

            for (int i = 0; i < 4; i++)
            {
                int nx = cur.x + dx[i];
                int ny = cur.y + dy[i];

                if (nx < 0 || nx >= w || ny < 0 || ny >= h) continue;
                if (map[nx, ny] == 0) continue;

                int cost = 1;
                if (map[nx, ny] == 2) cost = 3;
                if (map[nx, ny] == 3) cost = 5;

                int newCost = cur.cost + cost;

                if (newCost < dist[nx, ny])
                {
                    dist[nx, ny] = newCost;
                    pq.Push(new Node(nx, ny, newCost, cur));
                }
            }
        }

        if (targetNode == null) return null;

        List<Vector2Int> path = new List<Vector2Int>();
        Node trace = targetNode;
        while (trace != null)
        {
            path.Add(new Vector2Int(trace.x, trace.y));
            trace = trace.parent;
        }
        path.Reverse();
        return path;
    }

    void DrawMap()
    {
        for (int i = 0; i < currentMapObjs.Count; i++) Destroy(currentMapObjs[i]);
        currentMapObjs.Clear();

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3 pos = new Vector3(x, 0, y);
                GameObject obj = null;

                if (map[x, y] == 0) obj = Instantiate(wallObj, pos, Quaternion.identity);
                else if (map[x, y] == 1) obj = Instantiate(floorObj, pos, Quaternion.identity);
                else if (map[x, y] == 2) obj = Instantiate(treeObj, pos, Quaternion.identity);
                else if (map[x, y] == 3) obj = Instantiate(mudObj, pos, Quaternion.identity);

                if (obj != null) currentMapObjs.Add(obj);
            }
        }
    }

    IEnumerator ShowPath(List<Vector2Int> path)
    {
        foreach (var p in path)
        {
            Vector3 pos = new Vector3(p.x, 0.5f, p.y);
            GameObject obj = Instantiate(pathObj, pos, Quaternion.identity);
            pathObjs.Add(obj);
            yield return new WaitForSeconds(0.05f);
        }
    }
}