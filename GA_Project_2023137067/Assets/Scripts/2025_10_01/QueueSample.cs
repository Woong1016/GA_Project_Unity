using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Queue<string> queue = new Queue<string>();

        queue.Enqueue("첫번째");
        queue.Enqueue("두번째");
        queue.Enqueue("세번째");

        Debug.Log("=================================");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("=================================");

        Debug.Log("Peek :" + queue.Peek());

        Debug.Log("Dequeue " + queue.Dequeue());
        Debug.Log("Dequeue" + queue.Dequeue());

        Debug.Log("남은 데이터 수 :" + queue.Count);
        Debug.Log("=================================");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("=================================");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
