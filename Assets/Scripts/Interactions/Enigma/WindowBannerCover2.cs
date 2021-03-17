using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class WindowBannerCover2 : MonoBehaviour, IInteractable
{
    [SerializeField] WindowCoverEnigmaResolve resolverScript;
    [SerializeField] CrossHairController m_crossHairController;
    [SerializeField] GameObject coveringBanner;
    public GameObject item;
    public GameObject UI;
    private SpriteRenderer spriteRenderer;
    private Item banner;
    private Inventory inventory;
    private AudioSource soundFX;
    private bool covered;

    private void Awake()
    {
        coveringBanner.SetActive(false);
        covered = false;
    }

    void Start()
    {
        m_crossHairController = FindObjectOfType<CrossHairController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        banner = item.GetComponent<PickUpItem>().item;
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

        if (banner.pickedUp && !covered)
        {
            string message = "Cover window";
            m_crossHairController.DoorMode(message);
        }
        else if (!banner.pickedUp && !covered)
        {
            string message = "I should cover it";
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

        if (!covered)
        {
            m_crossHairController.StandardMode();
        }
    }

    public void OnInteract()
    {
        if (banner.pickedUp && !covered)
        {
            StartCoroutine(PlaySound());
        }
    }

    IEnumerator PlaySound()
    {
        soundFX.Play();
        yield return new WaitForSeconds(soundFX.clip.length);
        m_crossHairController.StandardMode();
        inventory.Use(banner);
        Destroy(item);
        coveringBanner.SetActive(true);
        covered = true;
        resolverScript.setSecondWindowCovered();
    }
}
