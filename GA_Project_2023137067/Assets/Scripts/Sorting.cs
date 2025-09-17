using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Sorting : MonoBehaviour
{
    Stopwatch sw = new Stopwatch();

    SelectionSortTest sel = new SelectionSortTest();
    BubbleSortTest bub = new BubbleSortTest();
    QuickSortTest quickSortTest = new QuickSortTest();

   


    public void OnselClickButton()
    {

        

         


    }
    public void OnbubClickButton()
    {



    }
    public void OnquickClickButton()
    {



    }
    // Start is called before the first frame update
    void Start()
    {
        int[] data1 = GenerateRandomArray(1000);
        sw.Reset();
        sw.Start();
        sel.StartSelectionSort(data1);

        sw.Stop();

        long selectionTime = sw.ElapsedMilliseconds;
        UnityEngine.Debug.Log(selectionTime);

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
}
