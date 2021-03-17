using UnityEngine;
using UnityEngine.Events;

public class Waiting : StateMachineBehaviour
{

    /*StateMachineMonsterController variables*/
    private StateMachineMonsterController controller;

    /*Animator variables*/
    private Animator stateMachine;

    /*MonsterAnimationController variables*/
    private MonsterAnimationController animatorMonster;

    /*UnityEvent variables*/
    private UnityEvent endWaitingEvent;

    private float timeWaitingStart = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animatorMonster == null)
            animatorMonster = animator.GetComponent<MonsterAnimationController>();
        if (stateMachine == null)
            stateMachine = animator;
        if (controller == null)
            controller = animator.GetComponent<StateMachineMonsterController>();
        if (endWaitingEvent == null)
        {
            endWaitingEvent = new UnityEvent();
            endWaitingEvent.AddListener(endWait);
        }
        animatorMonster.research(endWaitingEvent);

        timeWaitingStart = Time.time;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time - timeWaitingStart > 10)
        {
            controller.pickNewCurrentWaypoint();
            stateMachine.SetBool("isWaiting", false);
        }
    }

    public void endWait()
    {
        controller.pickNewCurrentWaypoint();
        stateMachine.SetBool("isWaiting", false);
    }
}
