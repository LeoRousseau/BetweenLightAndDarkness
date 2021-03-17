using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class DoorKeyInteraction : DoorInteraction
{
    [SerializeField] List<PickUpItem> m_keys;
    [SerializeField] private Inventory m_inventory;
    [SerializeField] bool stillOpenedAfterUnlock = true;

    private AudioSource soundFX;
    private bool unlockedWithKeys;

    private void Awake()
    {
        m_Door.LockDoor();
        unlockedWithKeys = false;
    }

    protected override void Start()
    {
        base.Start();


       if(m_inventory == null)
       {
            m_inventory = FindObjectOfType<Inventory>();
       }
       soundFX = GetComponent<AudioSource>();
    }

    public override void OnHoverEnter()
    {
        if (!unlockedWithKeys)
        {
            if (m_crossHairController == null)
            {
                Debug.LogWarning("no crosshair controller found");
                return;
            }

            if (!PickUpItem.AreItemsPickedUp(m_keys))
            {
                string message = "I need 2 keys";
                m_crossHairController.DoorMode(message);
            }

            else
            {
                string message = "Unlock main door";
                m_crossHairController.DoorMode(message);
            }
        }
        else 
        {
            base.OnHoverEnter();
        } 
    }

    public override void OnInteract()
    {
        if (!unlockedWithKeys)
        {
            if (PickUpItem.AreItemsPickedUp(m_keys))
            {
                foreach (PickUpItem key in m_keys.ToArray())
                {
                    m_inventory.Use(key.item);
                    m_keys.Remove(key);
                    Destroy(key);
                }
                m_Door.UnlockDoor();
                unlockedWithKeys = true;
                OnHoverEnter();
            } else
            {
                m_Door.OpenLockedDoor();
            }
        }
        else if (stillOpenedAfterUnlock)
        {
            base.OnInteract();
        }
        else 
        {
            if (!m_Door.Opened)
            {
                m_Door.OpenDoor();
                m_interactionCollider.enabled = false;
            }
            
        }
    }
}