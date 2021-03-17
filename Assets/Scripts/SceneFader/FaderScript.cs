using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class FaderScript : MonoBehaviour
{
    public static FaderScript m_instance  {get; private set; }


    private bool needFadeIn;
    private GameManager GM;

    Animator anim;
    int index;

    private void Awake()
    {

        if (m_instance == null)
        { 
            m_instance = this;
            m_instance.index = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_instance.needFadeIn = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (needFadeIn)
        {
            anim = GetComponent<Animator>();
            anim.SetBool("fo", false);
            anim.SetBool("fi", true);
            needFadeIn = false;
        }
    }

    public void FadeToLevel(int index)
    {
        this.index = index;
        anim.SetBool("fo", true);
    }

    public void onFadeComplete ()
    {
       GM.LauchScene(index);
    }

    public void fi()
    {
        Debug.Log("fi");
        anim = GetComponent<Animator>();
        anim.SetBool("fi", false);
    }

    public void fo()
    {
        anim.SetBool("fo", false);
    }


}
