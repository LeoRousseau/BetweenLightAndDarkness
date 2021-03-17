using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Attack : StateMachineBehaviour
{
    /*MonsterAnimationController variables*/
    private MonsterAnimationController animatorMonster;

    /*Animator variables*/
    private Animator stateMachine;

    /*NavMeshAgent variables*/
    private NavMeshAgent agent;

    /*UnityEvent variables*/
    private UnityEvent endAttackEvent = new UnityEvent();

    /*GameObject variables*/
    private GameObject player;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (stateMachine == null)
            stateMachine = animator;
        if (animatorMonster == null)
            animatorMonster = animator.GetComponent<MonsterAnimationController>();
        if (agent == null)
            agent = animator.GetComponent<NavMeshAgent>();
        if (player == null)
            player = animator.GetComponent<StateMachineMonsterController>().Player;
        if (endAttackEvent == null)
        {
            endAttackEvent = new UnityEvent();
            endAttackEvent.AddListener(endAttack);
        }
        animatorMonster.attack(endAttackEvent);
        agent.SetDestination(player.transform.position);
        agent.speed = 3;

    }


    public void endAttack()
    {
        stateMachine.SetBool("isAttacking", false);
    }


}
