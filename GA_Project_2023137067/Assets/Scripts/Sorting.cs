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
        
        sw.Reset();
        sw.Start(); 


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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
