using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PQueueTest : MonoBehaviour
{
    SimplePriorityQueue<string> queue = new SimplePriorityQueue<string>();
    Dictionary<string, float> unitSpeed = new Dictionary<string, float>();
    Dictionary<string, float> unitNextTurnTime = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Start()
    {


        //float PW = 100f / 5f;
        //float PM = 100f / 7f;
        //float PA = 100f / 10f;
        //float PC = 100f / 12f;
        //
        /// var queue = new SimplePriorityQueue<string>();
        //queue.Enqueue("전사", PW);
        //queue.Enqueue("마법사", PM);
        //queue.Enqueue("궁수", PA);
        //queue.Enqueue("홍길동", PC);

        unitSpeed.Add("전사", 5.0f);
        unitSpeed.Add("마법사", 7.0f);
        unitSpeed.Add("궁수", 10.0f);
        unitSpeed.Add("홍길동", 12.0f);

        foreach (var unit in unitSpeed)
        {
            string name = unit.Key;
            float speed = unit.Value;
            float firstTurnTime = 100.0f / speed;

            queue.Enqueue(name, firstTurnTime);
            unitNextTurnTime.Add(name, firstTurnTime);
        }


    }

    // Update is called once per frame
    void Update()
    {


        //
        //if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        Debug.Log(queue.Dequeue());
        //       
        //
        //    }

        if (Input.GetKeyDown(KeyCode.Space)) // 교수님은 8줄만으로 코드를 완성하셨다면서요..
                                                // 전 구글을 찾아보면서 몇일을 소비했습니다.. 
                                                // 도라에몽 암기빵이 시급해지는 느낌이였습니다
        {
            if (queue.Count == 0) return;

            string unitName = queue.Dequeue();
            float currentTime = unitNextTurnTime[unitName];

            Debug.Log($"[{currentTime:F2}초] {unitName}턴");

            float speed = unitSpeed[unitName];
            float cooldown = 100.0f / speed;
            float nextTurnTime = currentTime + cooldown;

            unitNextTurnTime[unitName] = nextTurnTime;
            queue.Enqueue(unitName, nextTurnTime);
        }


    }
}
