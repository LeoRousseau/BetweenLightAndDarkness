using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triforce;

public class UseFlask : MonoBehaviour, IInteractable
{
    [SerializeField] CrossHairController m_crossHairController;
    [SerializeField] ChemicalManager m_chemicalManager;
    private bool used;
    public string colorName;
    public Color color;
    private new Collider collider;
    public MixChemical mixChemical;

    // Start is called before the first frame update
    void Start()
    {
        m_crossHairController = FindObjectOfType<CrossHairController>();
        m_chemicalManager = FindObjectOfType<ChemicalManager>();
        used = false;
        color = transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color;
        collider = GetComponent<BoxCollider>();
    }

    public void Use()
    {
        used = true;
        collider.enabled = false;
        m_chemicalManager.Fill(color, colorName);
    }

    public void ResetFlask()
    {
        collider.enabled = true;
    }

    public void SetUse()
    {
        used = false;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void OnHoverEnter()
    {
        if (mixChemical.recipeData.pickedUp)
        {
            string message = "Use recipe";
            m_crossHairController.DoorMode(message);
        }
        else if (m_chemicalManager.GetReset())
        {
            string message = "I should try again";
            m_crossHairController.DoorMode(message);
        }
        else if (!used)
        {
            string message = "Add " + colorName + " chemical";
            m_crossHairController.DoorMode(message);
        }
    }

    public void OnInteract()
    {
        m_crossHairController.StandardMode();
        if (mixChemical.recipeData.pickedUp)
        {
            mixChemical.inventory.Use(mixChemical.recipeData);
            mixChemical.recipe.SetActive(true);

        }
        else if (m_chemicalManager.GetReset())
        {
            m_chemicalManager.EmptyErlen();
            used = false;
        }
        else if (!used)
        {
            Use();
        }
    }

    public void OnHoverExit()
    {
        m_crossHairController.StandardMode();
    }
}
