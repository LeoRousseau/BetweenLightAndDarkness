using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VMAnimationScript : MonoBehaviour
{
    private AudioSource audio;
    public Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void changeColor()
    {
        audio.Play();
        GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.4f, 0, 0);
        GetComponentInChildren<TextMeshProUGUI>().fontSizeMin = GetComponentInChildren<TextMeshProUGUI>().fontSize + 8;

    }

    public void resetColor()
    {
        GetComponentInChildren<TextMeshProUGUI>().color = defaultColor;
        GetComponentInChildren<TextMeshProUGUI>().fontSizeMin = GetComponentInChildren<TextMeshProUGUI>().fontSize - 8;
    }
}
