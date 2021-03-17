using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopTime()
    {
        GameObject.Find("DeathMenu").GetComponent<DeathMenuScript>().enableDeathMenu(true);
        Time.timeScale = 0f;
    }
}
