using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGymanisumDoorsResolve : MonoBehaviour
{
    private bool door1Open;
    private bool door2Open;
    public GameObject item;
    public GameObject UI;
    private Item key;
    private Inventory inventory;

    private void Awake()
    {
        door1Open = false;
        door2Open = false;
        key = item.GetComponent<PickUpItem>().item;
        inventory = UI.GetComponent<Inventory>();
    }

    public void setDoor1Opened()
    {
        door1Open = true;
        destroyKey();
    }

    public void setDoor2Opened()
    {
        door2Open = true;
        destroyKey();
    }

    private void destroyKey()
    { 
        if(door1Open && door2Open)
        {
            inventory.Use(key);
            Destroy(item);
        }
    }

}
