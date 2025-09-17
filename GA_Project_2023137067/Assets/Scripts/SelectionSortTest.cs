using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



public class SelectionSortTest : MonoBehaviour
{

    long selectionTime;


    // Start is called before the first frame update
    public void Start()
    {


        int[] data = GenerateRandomArray(100);

        Stopwatch sw = new Stopwatch();
        sw.Reset();
        sw.Start();
        StartSelectionSort(data);
        UnityEngine.Debug.Log("정렬중임");
        foreach (var item in data)
        {
            UnityEngine.Debug.Log(item);
        }
        sw.Stop();
       
        selectionTime = sw.ElapsedMilliseconds;

        
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

    public void StartSelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }

            int temp = arr[minIndex];
            arr[minIndex] = arr[i];
            arr[i] = temp;

            

            
        }

    }

    public int[] GenerateRandomArray(int size)
    {

        int[] arr = new int[size];
        System.Random rand = new System.Random();

        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000);

        }

        return arr;
    }
}
