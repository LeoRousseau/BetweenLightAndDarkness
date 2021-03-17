using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.Utility;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{

    public static GameManager m_instance { get; private set; }

    private AsyncOperation operation;
    private MainMenuScript menuScript;
    private FaderScript fader;
    public bool playerDead;
    public bool allowInteractions;


    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_instance.fader = GameObject.Find("SceneFader").GetComponent<FaderScript>();
        m_instance.playerDead = false;
        m_instance.allowInteractions = true;
    }

    public void KillPlayer()
    {
        if (!playerDead)
        {
            Triforce.PadLock[] padlocks = GameObject.FindObjectsOfType<Triforce.PadLock>();
            foreach (Triforce.PadLock pad in padlocks)
            {
                pad.desactivate();
            }
            allowInteractions = false;
            playerDead = true;
            GameObject.FindObjectOfType<DeathMenuScript>().gameObject.SetActive(true);
            GameObject.FindObjectOfType<Triforce.FirstPersonController>().GetComponent<CharacterController>().enabled = false;
            AudioSource[] sources = GameObject.FindObjectOfType<MusicManager>().GetComponents<AudioSource>();
            GameObject.FindObjectOfType<MusicManager>().active = false;
            foreach (AudioSource s in sources)
            {
                s.mute = true;
            }
            GameObject.FindObjectOfType<DeathMenuScript>().GetComponent<DeathMenuScript>().RunDeathMenu();
            playerDead = true;
        }
       
    }
    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene(GameObject menu)
    {
        menuScript = menu.GetComponent<MainMenuScript>();
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }


    IEnumerator LoadAsync(int ind)
    {
         operation = SceneManager.LoadSceneAsync(ind);

        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            menuScript.setSliderValue(Mathf.Clamp01(operation.progress / 0.9f));
            if (operation.progress >= 0.9f)
            {
                menuScript.SetLoadingText();
                menuScript.gameObject.GetComponent<Animator>().SetBool("Loaded", true);
                if (Input.anyKey)
                {
                    fader.GetComponent<FaderScript>().FadeToLevel(1);
                }
            }
            yield return null;
        }
    }


    public void LauchScene(int index)
    {
        
        allowInteractions = true;
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 0)
            operation.allowSceneActivation = true;
        else if (index == buildIndex)
            Restart();
        else
            SceneManager.LoadScene(index);
            
    }

    public void Victory()
    {
        allowInteractions = false;
        GameObject.FindObjectOfType<VictoryMenuScript>().GetComponent<VictoryMenuScript>().RunVictoryMenu();
    }


}
