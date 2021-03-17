using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAnimationController : MonoBehaviour
{
    public Animator animatorMonster;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void idle()
    {
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
    }

    public void walk()
    {
        animatorMonster.SetBool("isWalking", true);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
    }

    public void run()
    {
        animatorMonster.SetBool("isWalking", true);
        animatorMonster.SetBool("isRunning", true);
        animatorMonster.SetBool("isCrawling", false);
    }

    public void crawl()
    {
        animatorMonster.SetBool("isCrawling", true);
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
    }

    public void telekinesie()
    {
        animatorMonster.SetTrigger("telekinesie");
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
    }

    public void research()
    {
        animatorMonster.SetTrigger("search");
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
    }

    public void attack()
    {
        animatorMonster.SetTrigger("attack");
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
    }

    public void scream()
    {
        animatorMonster.SetTrigger("scream");
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
    }

    public void walk(UnityEvent eventEnd)
    {
        animatorMonster.SetBool("isWalking", true);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
        endState(eventEnd, "Walk forward", 0.95f);
    }

    public void run(UnityEvent eventEnd)
    {
        animatorMonster.SetBool("isWalking", true);
        animatorMonster.SetBool("isRunning", true);
        animatorMonster.SetBool("isCrawling", false);
        endState(eventEnd, "Run forward", 0.95f);
    }

    public void crawl(UnityEvent eventEnd)
    {
        animatorMonster.SetBool("isCrawling", true);
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        endState(eventEnd, "Crawl", 0.95f);
    }

    public void telekinesie(UnityEvent eventEnd)
    {
        animatorMonster.SetTrigger("telekinesie");
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
        endState(eventEnd, "Telekinesie", 0.55f);
    }

    public void research(UnityEvent eventEnd)
    {
        animatorMonster.SetTrigger("search");
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
        endState(eventEnd, "Research", 0.9f);
    }

    public void attack(UnityEvent eventEnd)
    {
        animatorMonster.SetTrigger("attack");
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
        endState(eventEnd, "Attack", 0.35f);
    }

    public void scream(UnityEvent eventEnd)
    {
        animatorMonster.SetTrigger("scream");
        animatorMonster.SetBool("isWalking", false);
        animatorMonster.SetBool("isRunning", false);
        animatorMonster.SetBool("isCrawling", false);
        endState(eventEnd, "Scream", 0.47f);
    }

    public void endState(UnityEvent endEvent, string name, float pourcent)
    {
        StartCoroutine(endStateIEnumerator(endEvent, name, pourcent));
    }


    public IEnumerator endStateIEnumerator(UnityEvent endEvent, string name, float pourcent)
    {
        while (!((animatorMonster.GetCurrentAnimatorStateInfo(0).IsName(name) && animatorMonster.GetCurrentAnimatorStateInfo(0).normalizedTime > pourcent)))
        {
            yield return new WaitForFixedUpdate();
        }
        endEvent.Invoke();
    }
}
