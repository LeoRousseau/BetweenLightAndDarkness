using UnityEngine;
using UnityEngine.AI;

public class Chase : StateMachineBehaviour
{
    /*Game Object variables*/
    private GameObject player;

    /*NavMeshAgent variables*/
    private NavMeshAgent agent;

    /*MonsterAnimationController variables*/
    private MonsterAnimationController animatorMonster;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialization of variables 
        if (animatorMonster == null)
            animatorMonster = animator.GetComponent<MonsterAnimationController>();
        if (player == null)
            player = animator.GetComponent<StateMachineMonsterController>().Player;
        if (agent == null)
            agent = animator.GetComponent<NavMeshAgent>();

        animatorMonster.run();
        agent.speed = 3;
    }


    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //set the destination of the NavMeshAgent to the new player position
        agent.SetDestination(player.transform.position);
    }
}
