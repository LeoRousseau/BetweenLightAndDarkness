using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class OpendDoor : StateMachineBehaviour
{
    /*StateMachineMonsterController variables*/
    public StateMachineMonsterController controller;

    /*NavMeshAgent variables*/
    public NavMeshAgent agent;

    /*Animator variables*/
    public Animator animatorMonster;

    /*UnityEvent variables*/
    public UnityEvent endOpenDoorEvent = new UnityEvent();

    public bool isOpening = false;

    private GameObject safePoint;

    private bool isInSafePointSide = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("isOpeningDoor");

        animator.SetBool("isOpeningDoor", true);
        if (animatorMonster == null)
            animatorMonster = animator;
        if (controller == null)
            controller = animator.GetComponent<StateMachineMonsterController>();
        if (agent == null)
            agent = animator.GetComponent<NavMeshAgent>();

        if (controller.doorToOpen.Locked)
            animatorMonster.SetBool("isOpeningDoor", false);

        //on cherche le safePoint de la cachette
        foreach (Transform child in controller.doorToOpen.gameObject.transform)
        {
            if (child.gameObject.name == "safePointExterior")
            {
                safePoint = child.gameObject;
            }
        }

        Vector3 sapePointVector = safePoint.transform.position - controller.doorToOpen.gameObject.transform.position;
        Vector3 monsterVector = controller.transform.position - controller.doorToOpen.gameObject.transform.position;

        isInSafePointSide = Vector3.Dot(sapePointVector, monsterVector) > 0;

        if (isInSafePointSide)
        {
            agent.destination = safePoint.transform.position;
            agent.speed = 1.5f;
        }
        else
        {
            agent.speed = 0f;
            agent.destination = controller.transform.position;
        }



    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("isOpeningDoorUpdate" + controller.isAtWaypoint(safePoint.transform.position, 1));
        if (isInSafePointSide)
        {
            if (controller.isAtWaypoint(safePoint.transform.position, 0.5f) && isOpening == false)
            {
                if (!controller.doorToOpen.Opened && !controller.doorToOpen.Moving && !controller.doorToOpen.Locked)
                {

                    endOpenDoorEvent.AddListener(endOpenDoor);
                    animator.GetComponent<MonsterAnimationController>().telekinesie(endOpenDoorEvent);
                    isOpening = true;

                    controller.lookAt(controller.doorToOpen.gameObject.transform.position, 4);
                }
                else
                {
                    animatorMonster.SetBool("isOpeningDoor", false);
                }
            }
            else
            {
                agent.destination = safePoint.transform.position;
            }
        }
        else
        {
            if (isOpening == false)
            {
                if (!controller.doorToOpen.Opened && !controller.doorToOpen.Moving && !controller.doorToOpen.Locked)
                {
                    endOpenDoorEvent.AddListener(endOpenDoor);
                    animator.GetComponent<MonsterAnimationController>().telekinesie(endOpenDoorEvent);
                    isOpening = true;
                    Debug.Log(controller.doorToOpen);
                    controller.lookAt(controller.doorToOpen.gameObject.transform.position, 4);
                }
                else
                {
                    animatorMonster.SetBool("isOpeningDoor", false);

                }
            }
        }
    }


    public void endOpenDoor()
    {
        isOpening = false;
        controller.doorToOpen.OpenDoor();
        animatorMonster.SetBool("isOpeningDoor", false);
    }
}
