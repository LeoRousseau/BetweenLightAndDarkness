using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilDollScript : MonoBehaviour
{
    private GameManager gm;
    private AudioSource source;

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip ScreamClip;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        source = GetComponent<AudioSource>();
    }

    public void Scream()
    {
        source.volume = 1f;
        source.PlayOneShot(ScreamClip);
    }

    public void Step()
    {
        source.volume = 0.4f;
        source.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
    }

    public void HurtPlayer()
    {
        if (!gm.playerDead)
            gm.KillPlayer();
    }
}
