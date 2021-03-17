using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator anim;
    private float translation;
    private float speed = 100.0f;
    private int crawl;
    private int startCrawling = 80;

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("isWalking", false);
        crawl = 0;
    }

    // Update is called once per frame
    void Update()
    {
        translation = Input.GetAxis("Vertical") * 2;

        if (Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                crawl++;
                if (crawl >= startCrawling)
                {
                    anim.SetBool("isCrawling", true);
                } 
                anim.SetBool("isRunning", true);
                transform.Translate(0, 0, 0.01f);
                
            } else
            {
                transform.Translate(0, 0, 0.005f);
                anim.SetBool("isRunning", false);
                anim.SetBool("isCrawling", false);
                anim.SetBool("isWalking", true);
                crawl = 0;
            }
                
        } else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }

        /*if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -0.1f);
        }*/
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(-Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }


    }
}
