using Triforce;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerSawInLocker : StateMachineBehaviour
{
    /*StateMachineMonsterController variables*/
    private StateMachineMonsterController controller;

    /*NavMeshAgent variables*/
    private NavMeshAgent agent;

    /*MonsterAnimationController variables*/
    private MonsterAnimationController animatorMonsterController;

    /*Animator variables*/
    private Animator animatorMonster;

    /*GameObject variables*/
    private GameObject safePoint;

    /*FirstPersonController variables*/
    private FirstPersonController player;

    /*UnityEvent variables*/
    private UnityEvent endOpeningEvent = new UnityEvent();

    /*bool variables*/
    public bool isOpening = false;

    /*Vector3 variables*/
    Vector3 destination = new Vector3();


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animatorMonster == null)
            animatorMonster = animator;
        if (animatorMonsterController == null)
            animatorMonsterController = animator.GetComponent<MonsterAnimationController>();
        if (controller == null)
            controller = animator.GetComponent<StateMachineMonsterController>();
        if (agent == null)
            agent = animator.GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = controller.Player.GetComponent<FirstPersonController>();
        }
        animatorMonsterController.run();
        endOpeningEvent.AddListener(endOpening);
        isOpening = false;
        agent.speed = 1.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //si on ne sait pas encore dans quelle cachette est le joueur
        if (safePoint == null && player.Hideout != null)
        {
            //on cherche le safePoint de la cachette
            foreach (Transform child in player.Hideout.transform)
            {
                if (child.gameObject.name == "safePointExterior")
                {
                    safePoint = child.gameObject;
                }
            }

            //on va plus loin que le safepoint pour que le monstre se tourne plus tard vers le joueur dans la cachette
            agent.destination = safePoint.transform.position;

        }

        //si on n'est pas en train d'ouvrir le casier et qu'on est devant le casier et qu'on connait la cachette du monstre
        if (player.Hideout != null && controller.isAtWaypoint(safePoint.transform.position, 0.5f) && !isOpening)
        {
            animatorMonsterController.telekinesie(endOpeningEvent);
            //destination = safePoint.transform.position + 1f * (safePoint.transform.position - player.Hideout.transform.position);
            isOpening = true;
            controller.lookAt(player.transform.position, 5);
            agent.speed = 0;
        }

        //si on est en train d'ouvrir le casier
        if (isOpening)
        {
            //agent.speed = 1f;
            //agent.destination = destination;
            //si le joueur sort on le tue
            if (!player.IsHidden && !player.IsHidding)
            {
                animatorMonster.SetBool("canAttack", true);
            }
        }

        //si on est pas en train d'ouvrir un casier et que le joueur sort du casier on sort de l'état playerSawInLocker
        if (!player.IsHidden && !player.IsHidding && !isOpening)
        {
            animatorMonster.SetBool("playerSawInLocker", false);
        }

    }

    public void endOpening()
    {
        animatorMonster.SetBool("canAttack", true);
        if (player.Hideout != null)
            player.Hideout.GetComponentInChildren<CacheInteractionTest>().OnInteract();
    }

}
