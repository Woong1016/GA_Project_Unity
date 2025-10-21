using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Stack<int> stack = new Stack<int>();
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);

        Debug.Log("============stack 1 ============");
        foreach (int num in stack)
            Debug.Log(num);
        Debug.Log("==================================");

        Debug.Log("Peek :" + stack.Peek());

        Debug.Log("Pop: " + stack.Pop());
        Debug.Log("Pop: " + stack.Pop());

        Debug.Log("=========stack 2 ============");
        foreach (int num in stack)
            Debug.Log(num);
        Debug.Log("=============================");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
