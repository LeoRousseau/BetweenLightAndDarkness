using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{

    private AudioSource source;

    public AudioClip music;
    public AudioClip buttonSound;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayMusic()
    {
        source.clip=music;
        source.Play();
    }

    public void PlayButtonSound ()
    {
        source.PlayOneShot(buttonSound);
    }
}
