using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Search2 : MonoBehaviour
{

    [Header("UI References")]
    public TMP_InputField searchInput;
    public Button linearSearchButton; 
    public Button binarySearchButton; 
    public GameObject itemPrefab;     
    public Transform contentParent;   

    private List<Item> items = new List<Item>();
    private List<GameObject> itemUIObjects = new List<GameObject>();
    private bool isSorted = false; 

    void Start()
    {
        InitializeStore();
       
        linearSearchButton.onClick.AddListener(OnLinearSearchButtonClick);
        binarySearchButton.onClick.AddListener(OnBinarySearchButtonClick);
    }

   
    void InitializeStore()
    {
        // Item_00 부터 Item_99 
        for (int i = 0; i < 100; i++)
        {
            // D2 포맷을 사용하여 "Item_00", "Item_01" 형식으로 생성한다는 뜻입니다. // D2 뜻 지금 알았어요
            items.Add(new Item($"Item_{i:D2}", 1));
        }

       
        foreach (var item in items)
        {
            GameObject newItemUI = Instantiate(itemPrefab, contentParent);
            newItemUI.GetComponentInChildren<TMP_Text>().text = item.itemName;
            itemUIObjects.Add(newItemUI);
        }
    }

   // 버튼 전용 함수
    public void OnLinearSearchButtonClick()
    {
        string target = searchInput.text;
        int foundIndex = FindItemLinear(target);
        UpdateUIVisibility(foundIndex);
    }

   // 버튼 전용 함수
    public void OnBinarySearchButtonClick()
    {
       
        if (!isSorted)
        {
            Quicksort(items, 0, items.Count - 1);
            isSorted = true;
            Debug.Log("List has been sorted for Binary Search.");
        }

        string target = searchInput.text;
        int foundIndex = FindItemBinary(target);
        UpdateUIVisibility(foundIndex);
    }

   
    private void UpdateUIVisibility(int foundIndex)
    {
        if (string.IsNullOrEmpty(searchInput.text))
        {
            // 
            foreach (var uiObject in itemUIObjects)
            {
                uiObject.SetActive(true);
            }
            return;
        }

        if (foundIndex != -1)
        {
            
            for (int i = 0; i < itemUIObjects.Count; i++)
            {
                itemUIObjects[i].SetActive(i == foundIndex);
            }
        }
        else
        {
            foreach (var uiObject in itemUIObjects)
            {
                uiObject.SetActive(false);
            }
        }
    }


   
    private int FindItemLinear(string target)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == target)
            {
                return i; // 찾았으면 해당 인덱스 반환
            }
        }
        return -1; // 못 찾았으면 -1 반환
    }

   
    private int FindItemBinary(string target)
    {
        int left = 0, right = items.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int cmp = items[mid].itemName.CompareTo(target);

            if (cmp == 0) return mid; // 찾았으면 해당 인덱스 반환
            else if (cmp < 0) left = mid + 1;
            else right = mid - 1;
        }
        return -1; // 못 찾았으면 -1 반환
    }

    // 여기까진 그냥 그대로 사용
   
    private void Quicksort(List<Item> list, int left, int right)
    {
        if (left >= right) return;
        int pivotIndex = Partition(list, left, right);
        Quicksort(list, left, pivotIndex - 1);
        Quicksort(list, pivotIndex + 1, right);
    }

   
    private int Partition(List<Item> list, int left, int right)
    {
        Item pivot = list[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (list[j].itemName.CompareTo(pivot.itemName) <= 0)
            {
                i++;
                Swap(list, i, j);
            }
        }
        Swap(list, i + 1, right);
        return i + 1;
    }

   
    private void Swap(List<Item> list, int a, int b)
    {
        Item temp = list[a];
        list[a] = list[b];
        list[b] = temp;
    }
}
