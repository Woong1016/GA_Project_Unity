using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Search2 : MonoBehaviour
{

    [Header("UI References (Tmp)")]
    public TMP_InputField inputItemCount;
    
    public TextMeshPro TMP;

    public GameObject prefabToSpawn;
    public Transform parentPanel;

    public Button button;
    public Button button2;

    private List<Item> items = new List<Item>();

    private long sortSteps;
    private long linearySteps;
    private long binarySteps;

    public void ofFindButton()
    {
        if (!int.TryParse(inputItemCount.text, out int itemCount)) itemCount = 100;


        items.Clear();

        for (int i = 0; i < itemCount; i++)
        {
            items.Add(new Item($"Item _ {Random.Range(0, itemCount):D5}", 1));
            GameObject newObject = Instantiate(prefabToSpawn, parentPanel);
            TMP.text = itemCount.ToString();
        }

       
    }


    public void Start1()
    {
        List<string> targets = new List<string>();
        linearySteps = 0;
        foreach (var t in targets)
        {
            linearySteps += FindItemLinearSteps(t);
        }
    }

    public void Start2()
    {
        List<string> targets = new List<string>();




        binarySteps = 0;
        foreach (var t in targets)
        {
            binarySteps += FindItemBinarySteps(t);
        }
    }

    private int FindItemLinearSteps(string target)

    {

        int steps = 0;
        foreach (Item item in items)
        {
            steps++;
            if (item.itemName == target)
            {
                return steps;
            }
        }
        return steps;
    }

    private int FindItemBinarySteps(string target)
    {
        int steps = 0;
        int left = 0, right = items.Count - 1;

        while (left <= right)
        {
            steps++;
            int mid = (left + right) / 2;
            int cmp = items[mid].itemName.CompareTo(target);

            if (cmp == 0) return steps;
            else if (cmp < 0) left = mid + 1;
            else right = mid - 1;

        }
        return steps;
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
