using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject imagePrefab;
    private GameObject grid;
    private GameObject image;

    private void Start()
    {
        grid = transform.GetChild(0).gameObject;
    }

    public void PickUp(Item item)
    {
        item.pickedUp = true;
        // Display item sprite in UI 
        image = Instantiate(imagePrefab, grid.transform);
        image.GetComponent<Image>().sprite = item.sprite;
        item.image = image;
    }

    public void Use(Item item)
    {
        item.pickedUp = false;
        // Remove item sprite from UI
        Destroy(item.image);
    }
}
