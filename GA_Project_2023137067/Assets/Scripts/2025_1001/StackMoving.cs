using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class StackMoving : MonoBehaviour
{
    public float speed = 5f;
    private Stack<Vector3> moveHistory;

    private Queue<Vector3> moveQueue;

    private bool isMoving = false;
    private Vector3 targetPos;

    public GameObject follow;
    // Start is called before the first frame update
    void Start()
    {
        moveHistory = new Stack<Vector3>();

        moveQueue = new Queue<Vector3>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveHistory.Push(transform.position);
        moveQueue.Enqueue(targetPos);
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        
        if (x != 0 || z != 0)
        {
            moveHistory.Push(transform.position);
            moveQueue.Enqueue(targetPos);
            moveQueue.Dequeue();
            moveQueue.Dequeue();

            Vector3 move = new Vector3(x, 0, z).normalized * speed * Time.deltaTime;
            //transform.position += move;
            new WaitForSeconds(2);
               
            while(moveQueue.Count > 0)
            {

                follow.transform.position = moveQueue.Dequeue();
                //new WaitForSeconds(2);


                //follow.transform.position = Vector3.MoveTowards(moveQueue.Dequeue(), moveHistory.Peek(), 5f * Time.deltaTime);
                
            }
           
        }
                

        if(Input .GetKey(KeyCode.Space))
        {
            if(moveHistory.Count > 0)
            {
                transform.position = moveHistory.Pop();
            }
        }
    }
}
