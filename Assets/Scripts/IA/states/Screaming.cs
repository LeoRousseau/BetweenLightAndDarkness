using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Screaming : StateMachineBehaviour
{
    /*Animator variables*/
    private Animator stateMachine;

    /*MonsterAnimationController variables*/
    private MonsterAnimationController animatorMonster;

    /*Transform variables*/
    public Transform currentWaypoint;

    /*NavMesh variables*/
    public NavMeshAgent agent;

    /*UnityEvent variables*/
    private UnityEvent endScreamEvent;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (stateMachine == null)
            stateMachine = animator;
        if (animatorMonster == null)
            animatorMonster = animator.GetComponent<MonsterAnimationController>();
        if (agent == null)
            agent = animator.GetComponent<NavMeshAgent>();

        if (endScreamEvent == null)
        {
            endScreamEvent = new UnityEvent();
            endScreamEvent.AddListener(screamingEnd);
        }
        animatorMonster.scream(endScreamEvent);
        agent.speed = 0;
    }


    public void screamingEnd()
    {
        stateMachine.SetBool("isChasing", true);
    }
}
