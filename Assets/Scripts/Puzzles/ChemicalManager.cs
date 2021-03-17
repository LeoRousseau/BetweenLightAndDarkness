using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalManager : MonoBehaviour
{
    private const int MAX_CHEMICALS = 3;
    public string[] recipe;
    public int fillingLevel;
    private GameObject solution;
    public UseFlask[] flasks;
    private Renderer solutionRenderer;
    private bool success;
    private new Collider collider;
    public AudioClip[] clips;
    private AudioSource soundFX;
    private bool reset;

    void Start()
    {
        fillingLevel = 0;
        solution = transform.GetChild(0).gameObject;
        solutionRenderer = solution.GetComponent<Renderer>();
        solution.SetActive(false);
        success = true;
        collider = GetComponent<Collider>();
        soundFX = GetComponent<AudioSource>();
    }
    
    public void Fill(Color color, string colorName)
    {
        fillingLevel++;
        CheckRecipe(color, colorName);
        StartCoroutine(PlayLiquidSound());
         
        switch (fillingLevel)
        {
            case 1:
                solution.SetActive(true);
                solution.transform.localScale = new Vector3(0.9f, 0.3f, 0.9f);
                solutionRenderer.material.color = color;
                break;

            case 2:
                solution.transform.localScale = new Vector3(0.9f, 0.6f, 0.9f);
                solutionRenderer.material.color += color;
                break;

            case MAX_CHEMICALS:
                solution.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                solutionRenderer.material.color += color;
                for (int i = 0; i < flasks.Length; i++)
                {
                    flasks[i].DisableCollider();
                }
                if (!success)
                {
                    ResetLab();
                }
                else
                {
                    StartCoroutine(PlaySuccessSound());
                    collider.enabled = true;
                }
                break;

            default:
                break;
        }
    }
    
    private void CheckRecipe(Color color, string colorName)
    {
        bool found = false;
        for (int i = 0; i < recipe.Length; i++)
        {
            if (colorName == recipe[i])
            {
                found = true;
            }
        }
        success = success && found;
        
    }
    
    private void ResetLab()
    {
        fillingLevel = 0;
        success = true;
        for (int i = 0; i < flasks.Length; i++)
        {
            flasks[i].ResetFlask();
        }
        reset = true;
    }

    public void EmptyErlen()
    {
        solution.SetActive(false);
        reset = false;
        for (int i = 0; i < flasks.Length; i++)
        {
            flasks[i].SetUse();
        }
    }

    private IEnumerator PlayLiquidSound()
    {
        soundFX.clip = clips[0];
        soundFX.Play();
        yield return new WaitForSeconds(soundFX.clip.length);
        
    }

    private IEnumerator PlaySuccessSound()
    {
        soundFX.clip = clips[2];
        soundFX.Play();
        yield return new WaitForSeconds(soundFX.clip.length);
        soundFX.clip = clips[1];

    }

    public bool GetReset()
    {
        return reset;
    }
}
