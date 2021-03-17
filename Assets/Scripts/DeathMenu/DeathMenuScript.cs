using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenuScript : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;

    private Triforce.FirstPersonController player;
    private Image imageScreen;
    private GameObject deathMC;
    private TextMeshProUGUI deathText;
    private GameObject musicManager;
    private GameObject point;
    private FaderScript fader;

    private bool run = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Triforce.FirstPersonController>();
        imageScreen = GetComponentInChildren<Image>();
        deathMC = GameObject.Find("DeathMenuContent");
        deathText = GetComponentInChildren<TextMeshProUGUI>();
        point = GameObject.Find("PlayerCanvas");
        fader = GameObject.Find("SceneFader").GetComponent<FaderScript>();
        deathMC.SetActive(false);
    }

    public void RunDeathMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
        run = true;
        player.GetComponentInChildren<Animation>().Play();
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetBool("Start", true);
        point.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MenuStarted()
    {
        GetComponent<Animator>().SetBool("Running", true);
    }

    public void enableDeathMenu(bool value)
    {
        player.MouseLocked(!value);
        player.enabled = !value;
        if (value)
            deathMC.SetActive(value);
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GetComponent<AudioSource>().Stop();
        fader.FadeToLevel(SceneManager.GetActiveScene().buildIndex);
    }

}
