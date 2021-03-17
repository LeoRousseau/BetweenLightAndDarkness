using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VictoryScript : MonoBehaviour
{
    GameManager gm;
    public PostProcessProfile profile;
    public AudioClip clip;
    bool isWin = false;
    float maxValue = 6f;
    ColorGrading grading;

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        grading = profile.GetSetting<ColorGrading>();
    }

    private void Update()
    {
        if (isWin)
        {
            if (grading.postExposure.value<maxValue)
                grading.postExposure.value += 0.2f;
            else
            {
                gm.Victory();
                isWin = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            isWin = true;
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}
