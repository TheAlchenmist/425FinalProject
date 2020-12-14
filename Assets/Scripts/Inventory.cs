using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using TMPro;

public class Inventory : MonoBehaviour
{
    //public GameObject tmp;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void AddToInventory(GameObject lootable)
    {
        if (inventory.ContainsKey(lootable.name))
        {
            inventory[lootable.name] += 1;
        }
        else
        {
            inventory.Add(lootable.name, 1);
        }
        // update UI
        //tmp.GetComponent<UIText>().UpdateInventoryText(Size());
        //tmp.SetText("Items in inventory: " + numItems);
    }

    public int Size()
    {
        int numItems = 0;

        foreach (KeyValuePair<string, int> kvp in inventory)
        {
            numItems += kvp.Value;
        }

        return numItems;
    }
}
