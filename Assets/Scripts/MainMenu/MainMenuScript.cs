using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    public GameObject LoadingMenu;

    private Slider slider;
   
    private TextMeshProUGUI loadingText;

    private GameObject sceneFader;

    //private Text
    private float val;
    Color imageColor;

    // Start is called before the first frame update
    void Start()
    {
        slider = LoadingMenu.GetComponentInChildren<Slider>();
        loadingText = LoadingMenu.GetComponentInChildren<TextMeshProUGUI>();
        sceneFader = GameObject.Find("SceneFader");
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void Play()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        LoadingMenu.SetActive(true);
        GetComponent<Animator>().SetBool("Played", true);

    }

    public void LoadLevel()
    {

       GameObject.Find("GameManager").GetComponent<GameManager>().LoadNextScene(this.gameObject);
    }

    public void Credits()
    {
        GetComponent<Animator>().SetBool("StartCredits", true);
        GetComponent<Animator>().SetBool("StopCredits", false);
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void setSliderValue (float val)
    {
        slider.value = val;
    }

    public void SetLoadingText ()
    {
        loadingText.text = "Press Any Key To Continue";
    }


    public void CreditStarted()
    {
        GetComponent<Animator>().SetBool("StartCredits", false);
        ButtonAnimationScript[] bas = GetComponentsInChildren<ButtonAnimationScript>();
        foreach (ButtonAnimationScript b in bas)
        {
            b.SetSize();
        }
    }

    public void Back()
    {
        GetComponent<Animator>().SetBool("StopCredits", true);
    }

}
