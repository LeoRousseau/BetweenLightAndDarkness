using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonAnimationScript : MonoBehaviour
{
    private MenuMusicManager mM;
    private bool isIn = false;
    // Start is called before the first frame update
    void Start()
    {
            mM = GameObject.Find("MusicManager").GetComponent<MenuMusicManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    

    public void changeColor()
    {
        mM.PlayButtonSound();
        GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.4f,0,0);
        GetComponentInChildren<TextMeshProUGUI>().fontSizeMin = GetComponentInChildren<TextMeshProUGUI>().fontSize + 8;
        isIn = true;
         
    }

    public void selectColor()
    {
        GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.2f, 0, 0);
    }

    public void reSelectedColor()
    {
        if (isIn)
        {
            GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.4f, 0, 0);
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.259f, 0.266f, 0.259f);

        }
        
    }

    public void SetSize()
    {
        GetComponentInChildren<TextMeshProUGUI>().fontSizeMin = 18;
        GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.259f, 0.266f, 0.259f);
        isIn = false;
    }
    public void resetColor()
    {
        GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.259f, 0.266f, 0.259f);
        GetComponentInChildren<TextMeshProUGUI>().fontSizeMin = GetComponentInChildren<TextMeshProUGUI>().fontSize - 8;
        isIn = false;
    }
}
