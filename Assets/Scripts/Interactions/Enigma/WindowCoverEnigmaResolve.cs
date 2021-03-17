using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowCoverEnigmaResolve : MonoBehaviour
{
    [Header("NumbersSprites")] 
    [SerializeField] GameObject[] spriteList;


    private bool firstWindowCovered;
    private bool secondWindowCovered;


    private void Awake()
    {

        firstWindowCovered = false;
        secondWindowCovered = false;
        hideFluorescentCode();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFirstWindowCovered()
    {
        firstWindowCovered = true;
        enigmaResolve();
    }

    public void setSecondWindowCovered()
    {
        secondWindowCovered = true;
        enigmaResolve();
    }

    private void enigmaResolve()
    {
        if(firstWindowCovered && secondWindowCovered)
        {
            showFluorescentCode();
        }
    }

    private void showFluorescentCode()
    {
        for(int i=0;i<spriteList.Length;i++)
        {
            spriteList[i].SetActive(true);
        }
    }

    private void hideFluorescentCode()
    {
        for (int i = 0; i < spriteList.Length; i++)
        {
            spriteList[i].SetActive(false);
        }
    }
}
