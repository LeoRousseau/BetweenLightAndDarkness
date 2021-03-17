using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public bool pickedUp;
    public GameObject image;

    public static bool areItemsPickedUp(List<Item> itemsList)
    {
        foreach (Item item in itemsList)
        {
            if (!item.pickedUp)
            {
                return false;
            }

        }
        return true;
    }
}


