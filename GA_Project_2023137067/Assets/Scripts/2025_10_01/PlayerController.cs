using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Stack<Vector3> positionHistory = new Stack<Vector3>();
    private bool isRewinding = false;

    void Start()
    {
        InvokeRepeating("RP", 0f, 0.05f);
    }

    void Update()
    {
        if (isRewinding) return;

        Move();

        if (Input.GetKeyDown(KeyCode.Space) && positionHistory.Count > 0)
        {
            StartCoroutine(Re(positionHistory.Count));
        }

        if (Input.GetKeyDown(KeyCode.R) && positionHistory.Count > 0)
        {
            StartCoroutine(Re(positionHistory.Count / 2));
        }
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            Vector3 moveDirection = new Vector3(h, 0, v).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }

    void RP()
    {
        positionHistory.Push(transform.position);
    }

    IEnumerator Re(int steps)
    {
        isRewinding = true;

        for (int i = 0; i < steps; i++)
        {
            if (positionHistory.Count > 0)
            {
                transform.position = positionHistory.Pop();
                yield return new WaitForSeconds(0.01f);
            }
        }

        isRewinding = false;
    }
}