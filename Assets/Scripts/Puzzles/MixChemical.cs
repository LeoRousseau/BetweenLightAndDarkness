using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class MixChemical : MonoBehaviour, IInteractable
{
    [SerializeField] CrossHairController m_crossHairController;
    public GameObject recipe;
    public Collider[] flasks;
    public Inventory inventory;
    public Item recipeData;
    private new Collider collider;

    void Start()
    {
        m_crossHairController = FindObjectOfType<CrossHairController>();
        recipeData = recipe.GetComponent<PickUpItem>().item;
        collider = GetComponent<BoxCollider>();
    }

    public void OnHoverEnter()
    {
        if (m_crossHairController == null)
        {
            Debug.LogWarning("no crosshair controller found");
            return;
        }

        if (recipeData.pickedUp)
        {
            string message = "Use recipe";
            m_crossHairController.DoorMode(message);
        }
        else
        {
            string message = "It's dangerous without a recipe";
            m_crossHairController.DoorMode(message);
        }
    }

    public void OnHoverExit()
    {
        if (m_crossHairController == null)
        {
            Debug.LogWarning("no crosshair controller found");
            return;
        }
        m_crossHairController.StandardMode();
    }

    public void OnInteract()
    {
        m_crossHairController.StandardMode();
        if (recipeData.pickedUp)
        {
            inventory.Use(recipeData);
            recipe.SetActive(true);
            
        }

        for (int i = 0; i < flasks.Length; i++)
        {
            flasks[i].GetComponent<BoxCollider>().enabled = true;
        }

        collider.enabled = false;
    }
    
}
