using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    public UnityEvent collisionTrigger;
    public bool triggerOnce;
    public bool triggerRandom;
    public int randomPercent;
    private bool isTriggered = false;
    public bool canBeHeard;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (triggerOnce && isTriggered)
                return;

            if (triggerRandom)
            {
                int perCent = Random.Range(0, 100);
                Debug.Log(perCent);
                if (perCent <= randomPercent)
                {
                    collisionTrigger.Invoke();

                    if (triggerOnce)
                        isTriggered = true;
                }
            }
            else if (triggerOnce)
            {
                collisionTrigger.Invoke();
                isTriggered = true;
            }
            else
                collisionTrigger.Invoke();
        }
    }
}
