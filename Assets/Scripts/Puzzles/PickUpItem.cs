using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class PickUpItem : MonoBehaviour, IInteractable
{
    [SerializeField] CrossHairController m_crossHairController;
    public GameObject UI;
    public Item item;
    private Inventory inventory;
    private AudioSource soundFX;
    private bool destroyed;

    void Start()
    {
        inventory = UI.GetComponent<Inventory>();
        m_crossHairController = FindObjectOfType<CrossHairController>();
        item.pickedUp = false;
        soundFX = GetComponent<AudioSource>();
        destroyed = false;
    }

    public void OnInteract()
    {
        if (!destroyed)
        {
            destroyed = true;
            inventory.PickUp(item);
            StartCoroutine(PlayPickUpSound());
        }
    }

    public void OnHoverEnter()
    {
        if (m_crossHairController == null)
        {
            Debug.LogWarning("no crosshair controller found");
            return;
        }

        string message = "Pick up " + item.name;
        m_crossHairController.DoorMode(message);    
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

    IEnumerator PlayPickUpSound()
    {
        soundFX.Play();
        yield return new WaitForSeconds(soundFX.clip.length);
        gameObject.SetActive(false);
    }


    public static bool AreItemsPickedUp(List<PickUpItem> pickedUpItemsList)
    {
        foreach (PickUpItem pItem in pickedUpItemsList)
        {
            if (!pItem.item.pickedUp)
            {
                return false;
            }

        }
        return true;
    }

}

    

