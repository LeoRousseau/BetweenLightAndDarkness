using UnityEngine;
using UnityEngine.AI;

public class Watch : StateMachineBehaviour
{
    /*Transform variables*/
    public Vector3 currentWaypoint;

    /*NavMesh variables*/
    public NavMeshAgent agent;

    /*MonsterAnimationController variables*/
    private MonsterAnimationController animatorMonster;

    /*StateMachineMonsterController variables*/
    public StateMachineMonsterController controller;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialization variables
        if (animatorMonster == null)
            animatorMonster = animator.GetComponent<MonsterAnimationController>();
        if (controller == null)
            controller = animator.GetComponent<StateMachineMonsterController>();
        if (agent == null)
            agent = animator.GetComponent<NavMeshAgent>();


        //choose the currentWaypoint randomly 
        if (controller.currentWaypoint == null)
        {
            controller.pickNewCurrentWaypoint();
            currentWaypoint = controller.currentWaypoint.position;
        }

        if (currentWaypoint != controller.currentWaypoint.position)
        {
            currentWaypoint = controller.currentWaypoint.position;
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(agent.destination, path);

            if (path.status == NavMeshPathStatus.PathInvalid || path.status == NavMeshPathStatus.PathPartial)
            {
                controller.currentWaypoint = null;
                Debug.Log("no path");
            }
        }
        agent.SetDestination(currentWaypoint);
        agent.speed = 1.5f;
        animatorMonster.walk();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (currentWaypoint != controller.currentWaypoint.position || currentWaypoint != agent.destination || controller.currentWaypoint.position != agent.destination)
        {
            currentWaypoint = controller.currentWaypoint.position;
            agent.SetDestination(currentWaypoint);
            //Debug.Log(path.status);
            if (agent.path.status == NavMeshPathStatus.PathInvalid || agent.path.status == NavMeshPathStatus.PathPartial)
            {
                controller.currentWaypoint = null;
                //Debug.Log("no path");
            }
        }


    }

}
