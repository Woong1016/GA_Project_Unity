using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;

public class SimpleMaze : MonoBehaviour
{
    public int width;
    public int height;
    int[,] map =
    {
        { 1,1,1,1,1},
        { 1,0,0,0,1},
        { 1,0,1,0,1},
        { 1,0,0,0,1},
        { 1,1,1,1,1}

    };

    bool[,] visited;
    Vector2Int goal = new Vector2Int(3, 3);
    Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };


    // Start is called before the first frame update
    void Start()
    {
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = 1;

            }
        }
        for (int x = 1; x < width-1; x++) // 이 코드로 벽제외 모두 뚫어놓음
        {
            for (int y = 1; y < height-1; y++)
            {

                
                map[x, y] = 0; // 교수님이 알려주심 

                map[x,y]= Random.Range(0, 1);

                if (map[x,y] == 1)
                {
                    //gameObject.
                }

            }
        }

        visited = new bool[map.GetLength(0), map.GetLength(1)];
            bool ok = SearchMaze(1, 1);
            Debug.Log(ok ? "출구 찾음!" : "출구 없음");

        

        // Update is called once per frame
        void Update()
        {

        }

        bool SearchMaze(int x, int y)
        {
            if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) return false;
            if (map[x, y] == 1 || visited[x, y]) return false;

            visited[x, y] = true;
            Debug.Log($"이동: ({x},{y})");

            if (x == goal.x && y == goal.y) return true;

            foreach (var d in dirs)
            {
                if (SearchMaze(x + d.x, y + d.y)) return true;
            }

            Debug.Log($"되돌아감 {x},{y}");
            return false;
        }
        void CarvePath(int x , int y)
        {
            
        }
    }
}
