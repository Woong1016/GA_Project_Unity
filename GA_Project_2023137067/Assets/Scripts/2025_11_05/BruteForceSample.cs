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
        runningRoutine = StartCoroutine(bruteForceRoutineTest());
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

        for (int i = 0; i < max; i++)
        {

        }




        for (int i = 0; i < max; i++)
        {
            string tryString = i.ToString("D4");
            tryCount++;

            if (tryString == secretpin)
            {
                sw.Stop();
                double seconds = sw.Elapsed.TotalSeconds;
                UnityEngine.Debug.Log($"[brute]성공! pin = {tryString} 시도수 = {tryCount} 소요 ={seconds:F3}초");
                runningRoutine = null;
                yield break;
            }

            if (i % 100 == 0)
            {
                yield return null;
            }
        }
        sw.Stop();
        UnityEngine.Debug.Log($"[brute] 모든 조합 시도완료 (발견실패)");
        runningRoutine = null;

    }
    
    IEnumerator bruteForceRoutineTest()
    {
        // 정신 나갈 것 같다

        Stopwatch sw = new Stopwatch();
        sw.Start();

       
        int totalCostLimit = 15;
        int quickShotMaxCount = 2, quickShotDamage = 6, quickShotCost = 2;
        int heavyShotMaxCount = 2, heavyShotDamage = 8, heavyShotCost = 3;
        int multiShotMaxCount = 1, multiShotDamage = 16, multiShotCost = 5;
        int tripleShotMaxCount = 1, tripleShotDamage = 24, tripleShotCost = 7;

      
        (int q, int h, int m, int t, int cost, int dmg) best;
        best = (q: 0, h: 0, m: 0, t: 0, cost: 0, dmg: -1);

        

        int totalCombinations = 0;

        
        for (int q = 0; q <= quickShotMaxCount; q++)
        {
            for (int h = 0; h <= heavyShotMaxCount; h++)
            {
                for (int m = 0; m <= multiShotMaxCount; m++)
                {
                    for (int t = 0; t <= tripleShotMaxCount; t++)
                    {
                        totalCombinations++;

                        
                        int currentCost = (q * quickShotCost) + (h * heavyShotCost) + (m * multiShotCost) + (t * tripleShotCost);
                        int currentDamage = (q * quickShotDamage) + (h * heavyShotDamage) + (m * multiShotDamage) + (t * tripleShotDamage);

                       
                        if (currentCost <= totalCostLimit)
                        {
                            
                            if (currentDamage > best.dmg)
                            {
                               
                                best = (q, h, m, t, currentCost, currentDamage);
                            }
                        }
                    }
                }
            }




        }

        
        sw.Stop();
        double seconds = sw.Elapsed.TotalSeconds;

      
        UnityEngine.Debug.Log($"총 {totalCombinations}개 조합 | 소요 시간 = {seconds:F6}초");
        UnityEngine.Debug.Log($" 퀵샷({best.q}) 헤비샷({best.h}) 멀티샷({best.m}) 트리플샷({best.t}), 코스트 {best.cost}/{totalCostLimit}, 데미지({best.dmg})");


        runningRoutine = null;
        yield break;
    }

}
