using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeta : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> itemNames;

    public List<ItemDeta> itemdeta;
    public int DropCount = 5;

    public void DropItem()
    {
        itemNames = new List<string>();
        itemNames.Add("점액");
        itemNames.Add("방울");
        itemNames.Add("potion");
        foreach (string itemName in itemNames)
        {
            Debug.Log("Item Name: " + itemName);
        }
    }


}
