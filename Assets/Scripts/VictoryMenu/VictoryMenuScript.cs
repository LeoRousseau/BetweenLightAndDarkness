using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenuScript : MonoBehaviour
{

    private GameObject player;
    private FaderScript fader;
    private GameObject playerCanvas;
    private GameObject VMC;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        fader  = GameObject.Find("SceneFader").GetComponent<FaderScript>();
        VMC = GameObject.Find("VictoryMenuContent");
        playerCanvas = GameObject.Find("PlayerCanvas");
        VMC.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunVictoryMenu()
    {
        player.GetComponent<Triforce.FirstPersonController>().MouseLocked(false);
        player.GetComponent<Triforce.FirstPersonController>().enabled = false;
        VMC.SetActive(true);
        playerCanvas.SetActive(false);
        Time.timeScale = 0f;
        
    }

    public void goToMenu()
    {
        Time.timeScale = 1f;
        fader.FadeToLevel(0);
    }

    public void Restart()
    {
        //GetComponent<AudioSource>().Stop();
        Time.timeScale = 1f;
        fader.FadeToLevel(SceneManager.GetActiveScene().buildIndex);
    }
}
