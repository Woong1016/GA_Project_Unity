using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

 
public class BruteForceSample : MonoBehaviour
{

    public Button startbutton;
    string secretpin;
    Coroutine runningRoutine;

    

    // Start is called before the first frame update
    void Start()
    {
        secretpin = Random.Range(0, 10000).ToString("D4");
        UnityEngine.Debug.Log($"[auth] 생성된 pin = {secretpin}");
    }

    public void OnStartButtonClicked()
    {
        if (runningRoutine != null)
        {
            UnityEngine.Debug.Log("[Brute] 이미 실행중입니다");
            return;

        }
        runningRoutine = StartCoroutine(bruteForceRoutine());
    }

    IEnumerator bruteForceRoutine()
    {
        UnityEngine.Debug.Log("[Brute] 시뮬레이션 시작");

        Stopwatch sw = new Stopwatch();
        sw.Start();

        int cost = 15;
        int tryCount = 0;
        int max = 10000;

        int quickShot = 2;
        int heavyShot = 3;
        int multShot = 5;
        int tripleShot = 7;

        for(int i = 0; i < max; i++)
        {

        }




        for (int i = 0; i < max; i++)
        {
            string tryString = i.ToString("D4");
            tryCount++;

            if(tryString == secretpin)
            {
                sw.Stop();
                double seconds = sw.Elapsed.TotalSeconds;
                UnityEngine.Debug.Log($"[brute]성공! pin = {tryString} 시도수 = {tryCount} 소요 ={seconds:F3}초");
                runningRoutine = null;
                yield break;
            }

            if(i%100 == 0)
            {
                yield return null; 
            }
        }
        sw.Stop();
        UnityEngine.Debug.Log($"[brute] 모든 조합 시도완료 (발견실패)");
        runningRoutine = null;

    }
    
}
