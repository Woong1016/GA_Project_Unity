using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueMoving : MonoBehaviour
{
    public float speed = 5f;

    private Queue<Vector3> moveQueue;

    private bool isMoving = false;
    private Vector3 targetPos;
    public GameObject follow;


    // Start is called before the first frame update
    void Start()
    {
        moveQueue = new Queue<Vector3>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {


            Vector3 move = new Vector3(x, 0, z).normalized * speed * Time.deltaTime;


                targetPos += move;
            moveQueue.Enqueue(targetPos);
            if(x != 0 || z != 0)
            {

            }
            follow.transform.position = moveQueue.Dequeue();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (!isMoving && moveQueue.Count > 0)
            {
                isMoving = true;
            }
        }
        else
        {
            if(moveQueue.Count > 0)
            {
                transform.position = moveQueue.Dequeue();
                isMoving = true;

            }
            else
            {
                isMoving = false;
            }
        }



        
    }
}
