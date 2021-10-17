using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot 
{
    public int index;
    public string name;
    public GameObject item;

    public InventorySlot(int index)
    {
        this.index = index;
    }

    public void Place(GameObject item)
    {
        this.item = item;
        name = item.name;
    }
}
