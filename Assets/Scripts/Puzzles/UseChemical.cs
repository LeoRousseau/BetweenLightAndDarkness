using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class UseChemical : MonoBehaviour, IInteractable
{
    [SerializeField] CrossHairController m_crossHairController;
    public GameObject erlen, UI, drawer;
    private Item chemical;
    private Inventory inventory;
    private bool chemicalUsed, openDrawer;
    private AudioSource soundFX;
    public AudioClip[] clips;
    public ParticleSystem steamEffect;
    private string message; // Cursor message
    private bool exit;

    void Start()
    {
        m_crossHairController = FindObjectOfType<CrossHairController>();
        chemical = erlen.GetComponent<PickUpItem>().item;
        inventory = UI.GetComponent<Inventory>();
        chemicalUsed = false;
        openDrawer = false;
        soundFX = GetComponent<AudioSource>();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(OpeningDrawer());
        }*/
    }

    public void OnHoverEnter()
    {
        if (m_crossHairController == null)
        {
            Debug.LogWarning("no crosshair controller found");
            return;
        }

        exit = false;

        if (chemical.pickedUp && !chemicalUsed)
        {
            message = "Pour chemical on rusty drawer";
            m_crossHairController.DoorMode(message);
        }
        else if (!chemical.pickedUp && !chemicalUsed)
        {
            message = "It seems rusty";
            m_crossHairController.DoorMode(message);
        }
        else if (chemicalUsed && openDrawer)
        {
            message = "Open drawer";
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

        if (!openDrawer)
        {
            exit = true;
            m_crossHairController.StandardMode();
        }
    }

    public void OnInteract()
    {
        // Use chemical to unlock drawer
        if (chemical.pickedUp && !chemicalUsed)
        {
            m_crossHairController.StandardMode();
            inventory.Use(chemical);
            Destroy(erlen);
            chemicalUsed = true;
            m_crossHairController.StandardMode();
            StartCoroutine(GenerateSmoke());
        }
        // Try to open drawer without having chemical
        else if (!chemical.pickedUp && !chemicalUsed)
        {
            StartCoroutine(PlayLockedSound());
        }
        // Open unlocked drawer
        else if (chemicalUsed && openDrawer)
        {
            openDrawer = false;
            m_crossHairController.StandardMode();
            StartCoroutine(OpeningDrawer());
        }
    }

    IEnumerator PlayLockedSound()
    {
        soundFX.clip = clips[0];
        soundFX.Play();
        yield return new WaitForSeconds(soundFX.clip.length);
    }

    IEnumerator GenerateSmoke()
    {
        soundFX.clip = clips[1];
        soundFX.Play();
        steamEffect.Play();
        yield return new WaitForSeconds(soundFX.clip.length);
        openDrawer = true;
        if (!exit)
        {
            message = "Open drawer";
            m_crossHairController.DoorMode(message);
        }
    }

    IEnumerator OpeningDrawer()
    {
        soundFX.clip = clips[2];
        soundFX.Play();
        while (drawer.transform.localPosition.z < 0.2f && soundFX.isPlaying)
        {
            drawer.transform.Translate(0, 0, 0.05f, drawer.transform);
            yield return new WaitForSeconds(Time.deltaTime);
            
        }
    }
}
