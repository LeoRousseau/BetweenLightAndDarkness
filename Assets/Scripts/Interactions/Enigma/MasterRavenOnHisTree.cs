using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class MasterRavenOnHisTree : MonoBehaviour, IInteractable
{
    [SerializeField] CrossHairController m_crossHairController;
    [SerializeField] GameObject flyingKey;
    [SerializeField] FlockChildEnigma _enigmaCrow;
    [SerializeField] ColliderTrigger _enigmaCrowSoundTrigger;
    public GameObject item;
    public GameObject UI;
    private SpriteRenderer spriteRenderer;
    private Item cheese;
    private Inventory inventory;
    private AudioSource soundFX;
    private bool cheeseGiven;

    private void Awake()
    {
        cheeseGiven = false;
    }

    void Start()
    {
        m_crossHairController = FindObjectOfType<CrossHairController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cheese = item.GetComponent<PickUpItem>().item;
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

        if (!cheese.pickedUp)
        {
            string message = "Croaw Croaw";
            m_crossHairController.DoorMode(message);
        }

        if (cheese.pickedUp && !cheeseGiven)
        {
            string message = "Give Me Croaw Cheese";
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

        if (!cheese.pickedUp)
        {
            m_crossHairController.StandardMode();
        }

        if (cheese.pickedUp && !cheeseGiven)
        {
            m_crossHairController.StandardMode();
        }
    }

    public void OnInteract()
    {
        if (cheese.pickedUp && !cheeseGiven)
        {
            flyingKey.AddComponent<Rigidbody>();
            flyingKey.GetComponent<Rigidbody>().detectCollisions = true;
            flyingKey.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            flyingKey.transform.parent = gameObject.transform;

            m_crossHairController.StandardMode();
            inventory.Use(cheese);
            Destroy(item);
            cheeseGiven = true;

            _enigmaCrowSoundTrigger.gameObject.SetActive(false);
            _enigmaCrow.FlyAway();
        }
        else if (!cheese.pickedUp)
        {
            _enigmaCrow.CrowSound();
        }
    }
}
