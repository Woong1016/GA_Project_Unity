using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{

    public List<Item> items = new List<Item> ();
    // Start is called before the first frame update
    void Start()
    {
        items.Add(new Item("sword"));
        items.Add(new Item("shield"));
        items.Add(new Item("potion"));

        Item found = FindItem("potion");

        if(found != null)
        {
            Debug.Log("찾은 아이템 : " + found.itemName);
            
        }
        else
        {
            Debug.Log("아이템을 찾을 수 없습니다");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item FindItem(string _itemName)
    {
        foreach (var item in items)
        {
            if(item.itemName == _itemName)
            {
                return item;

            }
            
        }
        return null;
    }

}
