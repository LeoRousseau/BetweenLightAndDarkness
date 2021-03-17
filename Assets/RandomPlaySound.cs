using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPlaySound : MonoBehaviour
{
    public float m_MinDelay;
    public float m_MaxDelay;
    public float m_MinPitch;
    public float m_MaxPitch;
    
    

    private float m_Remaining;

    // Start is called before the first frame update
    void Start()
    {
        CalcNewRemaining();
        GetComponent<AudioSource>().pitch = Random.Range(m_MinPitch, m_MaxPitch);
    }

    // Update is called once per frame
    void Update()
    {
        m_Remaining -= Time.deltaTime;
        if (m_Remaining < 0)
        {
            GetComponent<AudioSource>().Play();
            CalcNewRemaining();
        }
    }

    void CalcNewRemaining()
    {
        m_Remaining = Random.Range(m_MinDelay, m_MaxDelay);
    }
}
