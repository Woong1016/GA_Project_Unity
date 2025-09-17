using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



public class QuickSortTest : MonoBehaviour
{

    Stopwatch sw = new Stopwatch();
    // Start is called before the first frame update
    public void Start()
    {
        int[] data = GenerateRandomArray(100);
        StartQuickSort(data, 0 , data.Length -1);
        UnityEngine.Debug.Log("정렬중임 퀵정렬");
        sw.Reset();
        sw.Start();
        QuickSortTest.StartQuickSort(data , 0 , data.Length-1);
        foreach (var item in data)
        {
            UnityEngine. Debug.Log(item);
        }
        sw.Stop();
        long QuickTime = sw.ElapsedMilliseconds;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public static void StartQuickSort(int[] arr , int low , int high)
    {
        if(low < high)
        {
            int pivotIndex = Partition(arr, low, high);
        }
    }


    private static int Partition(int[] arr ,int low , int high)
    {
        int pivot = arr[high];
        int i = (low - 1);

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;

                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
        return i + 1; 
    }    

}
