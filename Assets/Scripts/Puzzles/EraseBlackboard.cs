using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class EraseBlackboard : MonoBehaviour, IInteractable
{
    [SerializeField] CrossHairController m_crossHairController;
    [SerializeField] const int dustDuration = 3;
    public GameObject item;
    public GameObject UI;
    public Sprite codeText;
    private SpriteRenderer spriteRenderer;
    public ParticleSystem chalkDustEffect;
    private Item eraser;
    private Inventory inventory;
    private bool textErased;
    private AudioSource soundFX;


    void Start()
    {
        chalkDustEffect.Stop();
        m_crossHairController = FindObjectOfType<CrossHairController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        eraser = item.GetComponent<PickUpItem>().item;
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

        if (eraser.pickedUp && !textErased)
        {
            string message = "Erase blackboard";
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

        if (eraser.pickedUp && !textErased)
        {
            m_crossHairController.StandardMode();
        }
    }

    public void OnInteract()
    {
        if (eraser.pickedUp && !textErased)
        {
            m_crossHairController.StandardMode();
            inventory.Use(eraser);
            Destroy(item);
            StartCoroutine(GenerateChalkDust());
        }
    }

    IEnumerator GenerateChalkDust()
    {
        print("play dust");
        soundFX.Play();
        chalkDustEffect.Play();
        yield return new WaitForSeconds(dustDuration);
        spriteRenderer.sprite = codeText;
        chalkDustEffect.Stop();
        print("stop dust");
        textErased = true;
    }
}
