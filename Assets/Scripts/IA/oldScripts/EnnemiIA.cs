using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnnemiIA : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform[] waypoint;
    public float StartWaitTime;
    public GameObject Player;
    private float waitTime;
    private int waypointIndice;
    private float fieldOfView = 80;
    private float rangeOfView = 4;
    private float rangeOfAttack = 1.5f;
    private bool chasing = false;
    // Start is called before the first frame update

    void Start()
    {
        waypointIndice = 0;
        waitTime = StartWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (chasing)
        {
            if (canAttack())
            {
                Debug.Log("Attack");
            }
            else
            {
                agent.SetDestination(Player.transform.position);
            }
        }
        else
        {
            if (CanSeePlayer())
            {
                chasing = true;
            }
            else if (Vector3.Distance(this.transform.position, waypoint[waypointIndice].transform.position) > 0.2)
            {
                agent.SetDestination(waypoint[waypointIndice].transform.position);
            }
            else
            {
                if (waitTime >= 0)
                {
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    selectNewWaypoint();
                    waitTime = StartWaitTime;
                }

            }
        }

    }


    protected bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 rayDirection = Player.transform.position - transform.position;

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfView * 0.5f)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, rangeOfView))
            {
                return (hit.transform.CompareTag("Player"));
            }
        }

        return false;
    }

    public bool canAttack()
    {
        if (Vector3.Distance(this.transform.position, Player.transform.position) <= rangeOfAttack) { return true; }
        else { return false; }
    }


    public void selectNewWaypoint()
    {
        int ind = Random.Range(0, waypoint.Length);
        waypointIndice = ind;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, rangeOfView);
    }

}
