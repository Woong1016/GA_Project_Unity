using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Sorting : MonoBehaviour
{

    public Text text;

    Stopwatch sw = new Stopwatch();

    //SelectionSortTest sel = new SelectionSortTest();
    //BubbleSortTest bub = new BubbleSortTest();
    //QuickSortTest quickSortTest = new QuickSortTest();

    //static은 일부러 지웠습니다 인스턴스화 헤서 아직까진 더 편해서
    //메모리 관련 누수 오류 너무 많이 떠서 다시 static으로

    public void OnClickselButton()
    {
        ClearConsole();
        sw.Reset();

        int[] data1 = GenerateRandomArray(10000);
        
        sw.Start();
        //sel.StartSelectionSort(data1);
        SelectionSortTest.StartSelectionSort(data1);
        sw.Stop();

        float selectionTime = sw.ElapsedMilliseconds;
        text.text = $"정렬시간 : {selectionTime:F1} ms";  // f1은 .0까지 보고싶어서
        //UnityEngine.Debug.Log(text.text);

        foreach (var item in data1)
         {
            UnityEngine.Debug.Log(item);
         }

        
    }
    public void OnClickbubButton()
    {
        ClearConsole();
        sw.Reset();

        int[] data2 = GenerateRandomArray(2000);

        sw.Start();
        //bub.StartBubbleSort(data2);
        BubbleSortTest.StartBubbleSort(data2);

        sw.Stop();

        float bubectionTime = sw.ElapsedMilliseconds;
        text.text = $"정렬시간 : {bubectionTime:F1} ms";  // f1은 .0까지 보고싶어서
        //UnityEngine.Debug.Log(text.text);
        foreach (var item in data2)
        {
            UnityEngine.Debug.Log(item);
        }
    }
    public void OnClickquickButton()
    {
        ClearConsole();
        sw.Reset();

        int[] data3 = GenerateRandomArray(10000);

        sw.Start();
        QuickSortTest.StartQuickSort(data3, 0, data3.Length - 1);

        sw.Stop();

        float quick = sw.ElapsedMilliseconds;
        text.text = $"정렬시간 : {quick:F1} ms";  // f1은 .0까지 보고싶어서
        //UnityEngine.Debug.Log(text.text);
       foreach (var item in data3)
       {
           UnityEngine.Debug.Log(item);
       }


    }
    // Start is called before the first frame update
    void Start()
    {
        

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

    private void ClearConsole()
    {
#if UNITY_EDITOR // 콘솔 호출할때마다 클리어시켜주는 함수
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
#endif
    }
}
