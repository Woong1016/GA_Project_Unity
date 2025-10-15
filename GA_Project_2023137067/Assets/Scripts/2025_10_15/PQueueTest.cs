using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PQueueTest : MonoBehaviour
{
    SimplePriorityQueue<string>queue = new SimplePriorityQueue<string>();

    // Start is called before the first frame update
    void Start()
    {

        
        queue.Enqueue("PlayerA", -5);
        queue.Enqueue("PlayerB", -7);
        queue.Enqueue("PlayerC", -10);
        queue.Enqueue("PlayerD", -12);


        //var queue = new SimplePriorityQueue<string>();
        //queue.Enqueue("PlayerA", 5);
        //queue.Enqueue("PlayerB", 7);
        //queue.Enqueue("PlayerC", 10);
        //queue.Enqueue("PlayerD", 12);
        //
        //
        //while (queue.Count > 0)
        //{
        //    Debug.Log(queue.Dequeue());
        //}
    }

    // Update is called once per frame
    void Update()
    {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(queue.Dequeue());

            queue.Enqueue("PlayerD", -12);


            }
        
       
    }
}
