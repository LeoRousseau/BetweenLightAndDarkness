using Triforce;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Extinguish : StateMachineBehaviour
{

    /*StateMachineMonsterController variables*/
    public StateMachineMonsterController controller;

    /*NavMeshAgent variables*/
    public NavMeshAgent agent;

    /*Animator variables*/
    public Animator animatorMonster;

    /*UnityEvent variables*/
    public UnityEvent endExtinguishEvent = new UnityEvent();


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animatorMonster == null)
            animatorMonster = animator;
        if (controller == null)
            controller = animator.GetComponent<StateMachineMonsterController>();
        if (agent == null)
            agent = animator.GetComponent<NavMeshAgent>();

        endExtinguishEvent.AddListener(endExtinguish);
        animator.GetComponent<MonsterAnimationController>().telekinesie(endExtinguishEvent);
        agent.speed = 0;
    }


    public void endExtinguish()
    {
        if (controller.currentWaypoint.GetComponent<InteractiveLight>())
            controller.currentWaypoint.GetComponent<InteractiveLight>().TurnOff();
        animatorMonster.SetBool("isExtinguishing", false);
    }
}
