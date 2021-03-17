using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class OpenGymnasiumDoor1 : MonoBehaviour,IInteractable
{
    [SerializeField] OpenGymanisumDoorsResolve resolverScript;
    [SerializeField] CrossHairController m_crossHairController;
    [SerializeField] CustomDoor door1;
    [SerializeField] GameObject doorBlocker;
    public GameObject item;
    public GameObject UI;
    private SpriteRenderer spriteRenderer;
    private Item key;
    private Inventory inventory;
    private AudioSource soundFX;
    public AudioClip[] clips;
    private bool opened;

    private void Awake()
    {
        door1.LockDoor();
        opened = false;
    }

    void Start()
    {
        m_crossHairController = FindObjectOfType<CrossHairController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        key = item.GetComponent<PickUpItem>().item;
        inventory = UI.GetComponent<Inventory>();
        soundFX = GetComponent<AudioSource>();
    }

    public void OnHoverEnter()
    {
        if (m_crossHairController == null)
        {
            Debug.LogWarning("no crosshair controller found");
            return;
        }

        if (!key.pickedUp)
        {
            string message = "I need a key";
            m_crossHairController.DoorMode(message);
        }

        if (key.pickedUp && !opened)
        {
            string message = "Unlock door";
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

        if (!key.pickedUp)
        {
            m_crossHairController.StandardMode();
        }

        if (key.pickedUp && !opened)
        {
            m_crossHairController.StandardMode();
        }
    }

    public void OnInteract()
    {
        if (key.pickedUp && !opened)
        {
            m_crossHairController.StandardMode();
            /*inventory.Use(key);
            Destroy(item);*/
            StartCoroutine(PlayUnlockSound());
        }
        else if (!opened)
        {
            soundFX.clip = clips[0];
            soundFX.Play();
        }
    }

    private IEnumerator PlayUnlockSound()
    {
        soundFX.clip = clips[1];
        soundFX.Play();
        yield return new WaitForSeconds(soundFX.clip.length);
        opened = true;
        doorBlocker.SetActive(false);
        door1.UnlockDoor();
        resolverScript.setDoor1Opened();

    }
}
