using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockChildEnigma : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] Transform _finalDestination;
    [SerializeField] Animation _animation;

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _mainCrowSound;
    [SerializeField] AudioClip _flyAwaySound;

    public void FlyAway()
    {
        _audioSource.clip = _flyAwaySound;
        _audioSource.Play();
        _animation.CrossFade("CrowFlap", .2f);
        StartCoroutine(CFlyAway());
    }
    public void CrowSound()
    {
        if (_audioSource.isPlaying)
            return;
        _audioSource.PlayOneShot(_mainCrowSound);
    }
    IEnumerator CFlyAway()
    {
        while(Vector3.Distance(this.transform.position, _finalDestination.position) > 0.0001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _finalDestination.position, Time.deltaTime * speed);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
