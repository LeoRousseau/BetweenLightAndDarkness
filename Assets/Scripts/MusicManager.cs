using System.Collections;
using UnityEngine;

/*Music Manager: handle the music of the game*/
public class MusicManager : MonoBehaviour
{
    /*AudioClip variables*/
    public AudioClip audioClipMonsterClose;
    public AudioClip audioClipMonsterFar;
    public AudioClip audioClipMonsterChase;

    /*Audio Source variables*/
    public AudioSource audioSource;

    /*Bool variables*/
    private bool isSettingMusic = false;
    public bool active = true;

    /*Float variables*/
    private float targetVolume;

    /*Time variables*/
    private float lastUpdateVolumeTime;


    // Start is called before the first frame update
    void Start()
    {
        //Initializtion of time variables
        lastUpdateVolumeTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        //if we don't change music we approach the volume of the audioSource to the targetVolume 
        if (!isSettingMusic && Time.time - lastUpdateVolumeTime > 0.01f && active)
        {
            if (Mathf.Abs(targetVolume - audioSource.volume) < 0.01f)
            {
                audioSource.volume = targetVolume;
            }
            lastUpdateVolumeTime = Time.time;
            if (targetVolume > audioSource.volume)
            {
                audioSource.volume += 0.01f;
            }
            else
            {
                audioSource.volume -= 0.01f;
            }
        }

        if (!active)
        {
            audioSource.volume = 0;
            targetVolume = 0;
        }
    }

    //Set the targetVolume
    public void setTargetVolume(float volume)
    {
        targetVolume = volume;
    }

    /*METHODES OF TEST*/
    /*Methodes which return true if it's possible to change music for the one asked */
    public bool canSetMusicMonsterClose()
    {
        return audioSource.clip == audioClipMonsterClose || isSettingMusic;
    }

    public bool canSetMusicMonsterFar()
    {
        return audioSource.clip == audioClipMonsterFar || isSettingMusic;
    }

    public bool canSetMusicMonsterChase()
    {
        return audioSource.clip == audioClipMonsterChase || isSettingMusic;
    }

    /*SET MUSIC METHODES*/
    public void setMusicMonsterCLose(float time)
    {
        StartCoroutine(setMusic(audioClipMonsterClose, time));
    }

    public void setMusicMonsterFar(float time)
    {
        StartCoroutine(setMusic(audioClipMonsterFar, time));
    }

    public void setMusicMonsterChase(float time)
    {
        StartCoroutine(setMusic(audioClipMonsterChase, time));
    }

    //Coroutine which change the music with a fade out and a fade in
    public IEnumerator setMusic(AudioClip clip, float time)
    {
        //initialization
        isSettingMusic = true;
        float volume = 0;
        float startVolume = audioSource.volume;


        //creation of the new audioSource with the new clip
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = clip;
        newAudioSource.volume = 0;
        newAudioSource.Play();
        newAudioSource.loop = true;

        //while the fade is not over
        while (volume < targetVolume)
        {
            audioSource.volume = startVolume - (volume * startVolume / targetVolume);   //Fade out volume
            newAudioSource.volume = volume;                                             //Fade in volume
            yield return new WaitForSeconds(0.01f);                                     //Wait for few secondes
            volume += targetVolume * 0.01f / time;                                      //update of the Fade in volume  variable
        }

        //reset and return
        audioSource.volume = 0;
        newAudioSource.volume = targetVolume;
        Destroy(audioSource);                       //Replace the last audioSource by the new one
        audioSource = newAudioSource;
        isSettingMusic = false;
    }
}
