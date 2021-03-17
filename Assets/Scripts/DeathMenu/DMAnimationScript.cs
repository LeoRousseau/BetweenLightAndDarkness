using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DMAnimationScript : MonoBehaviour
{

    private AudioSource audio;
    // Start is called before the first frame update

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = 0.2f;
    }


    public void changeColor()
    {
        audio.Play();
        GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.4f, 0, 0);
        GetComponentInChildren<TextMeshProUGUI>().fontSizeMin = GetComponentInChildren<TextMeshProUGUI>().fontSize + 8;

    }

    public void resetColor()
    {
        GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.259f, 0.266f, 0.259f);
        GetComponentInChildren<TextMeshProUGUI>().fontSizeMin = GetComponentInChildren<TextMeshProUGUI>().fontSize - 8;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
