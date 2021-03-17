using System.Collections;
using System.Collections.Generic;
using Triforce;
using UnityEngine;

public class StateMachineMonsterController : MonoBehaviour
{

    /*Game Object variables*/
    public GameObject Player;
    public GameObject monsterHead;
    private GameObject audioTarget = null;
    private GameObject lastPlacePlayerSeen = null;

    /*Animator vriables*/
    public Animator stateMachineMonster;

    /*Float variables*/
    public float fieldOfView = 80;
    public float rangeOfView = 4;
    public float rangeOfViewInDark = 10;
    public float rangeOfAttack = 1.5f;
    public float rangeOfHearing = 3.0f;
    public float farDistanceMusicMonsterClose = 10;
    public float farDistanceMusicMonsterFar = 15;
    public float distanceToLight = 10;

    /*Time variables*/
    public float timeToPurchase = 1.5f;
    private float lastTimeSeeingPlayer = 0;

    /*MusicManager variables*/
    public MusicManager musicManager;

    /*CustomDoorVariables*/
    public CustomDoor doorToOpen;

    /*Transform variables*/
    public Transform currentWaypoint;
    public Transform waypoints;

    /*variables for cheats*/
    public bool isPlayerVisible = true;
    public bool playerSawInLocker = false;
    public bool sawPreviousFrame = false;

    /*FirstPersonController variables*/
    private FirstPersonController fpsController;

    /*Collections*/
    private List<Transform> lastPlacesSawPlayer = new List<Transform>();
    private InteractiveLight[] lightTab;
    private Transform[] waypointsArray;

    private float timeBugHandler;
    private int currentState;

    // Start is called before the first frame update
    void Start()
    {
        timeBugHandler = Time.time;
        currentState = stateMachineMonster.GetCurrentAnimatorStateInfo(0).fullPathHash;
        //we assign all the variables that we need 
        if (waypoints)
            waypointsArray = waypoints.GetComponentsInChildren<Transform>();
        if (Player)
            fpsController = Player.GetComponent<FirstPersonController>();

        lightTab = GameObject.FindObjectsOfType<InteractiveLight>();
        audioTarget = new GameObject();
        lastPlacePlayerSeen = new GameObject();
        audioTarget.name = "audioTarget";
        lastPlacePlayerSeen.name = "lastPlacePlayerSeen";

    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachineMonster.GetCurrentAnimatorStateInfo(0).fullPathHash != currentState)
        {
            timeBugHandler = Time.time;
            currentState = stateMachineMonster.GetCurrentAnimatorStateInfo(0).fullPathHash;
            //Debug.Log("test1");
        }
        else
        {
            //Debug.Log("test2" + (Time.time - timeBugHandler));
            if (Time.time - timeBugHandler > 150)
            {
                transform.position = waypointsArray[1].position;
                stateMachineMonster.SetBool("playerSawInLocker", false);
                stateMachineMonster.SetBool("canSeePlayer", false);
                stateMachineMonster.SetBool("isWaiting", false);
                stateMachineMonster.SetBool("isChasing", false);
                stateMachineMonster.SetBool("isAttacking", false);
                stateMachineMonster.SetBool("isOpeningDoor", false);
                stateMachineMonster.SetBool("canOpenDoor", false);
                stateMachineMonster.SetBool("isExtinguishing", false);
                timeBugHandler = Time.time;
            }
        }
        //each frame we change the state machine variables

        //if the monster saw the player hide, it's the playerSawInLocker state
        if (playerSawInLocker)
        {
            stateMachineMonster.SetBool("playerSawInLocker", true);
            stateMachineMonster.SetBool("canSeePlayer", true);
            stateMachineMonster.SetBool("isWaiting", false);
            stateMachineMonster.SetBool("isChasing", true);
            stateMachineMonster.SetBool("isAttacking", false);
            stateMachineMonster.SetBool("isOpeningDoor", false);
            stateMachineMonster.SetBool("canOpenDoor", false);
            stateMachineMonster.SetBool("isExtinguishing", false);

        }
        else
        {
            //else we are not in the playerSawInLocker state
            stateMachineMonster.SetBool("playerSawInLocker", false);

            //and we need to check if we can see the player and so if we chase him
            //not chase mode
            if (!CanSeePlayer())
            {
                //if the monster chased the player at last update so we check the last place we saw him and the waypoints near the last place we saw the player
                if (stateMachineMonster.GetBool("isChasing"))
                {
                    currentWaypoint = lastPlacePlayerSeen.transform;
                    nearestWaypoints(lastPlacePlayerSeen.transform.position, 3);
                }
                //we check if we need to go to an other waypoint each update in case the player played sounds or if the player have light a candle
                pickDestination();
                stateMachineMonster.SetBool("canSeePlayer", false);
                stateMachineMonster.SetBool("isChasing", false);
                stateMachineMonster.SetBool("canAttack", false);
                stateMachineMonster.SetBool("isAttacking", false);
                stateMachineMonster.SetBool("canOpenDoor", mustOpenDoor());

                //we check the kind of waypoint we try to reach
                string stateName;
                if (currentWaypoint.GetComponent<InteractiveLight>())
                {
                    stateName = "isExtinguishing";
                }
                else
                {
                    stateName = "isWaiting";
                }

                //if we are at waypoint and not opening a door we extinguish light or wait at waypoint, depending of waypoint kind
                if (!stateMachineMonster.GetBool("canOpenDoor"))
                {

                    stateMachineMonster.SetBool(stateName, isAtWaypoint());
                }
                else
                {
                    stateMachineMonster.SetBool(stateName, false);
                }

            }

            //chase mode
            else
            {
                //if we see the player hide we go in the hide mode

                stateMachineMonster.SetBool("canSeePlayer", true);
                stateMachineMonster.SetBool("isWaiting", false);
                if (stateMachineMonster.GetBool("isChasing"))
                {
                    stateMachineMonster.SetBool("canAttack", canAttack());
                }
                else
                {
                    stateMachineMonster.SetBool("canAttack", false);
                    stateMachineMonster.SetBool("isAttacking", false);
                }

            }
        }

        //each update we update the music
        updateMusic();
    }

    /*nearestWaypoints put the numberWaypoints closests waypoints of position in lastPlacesSawPlayer*/
    public void nearestWaypoints(Vector3 position, int numberWaypoints)
    {
        //we clear the current  lastPlacesSawPlayer list
        lastPlacesSawPlayer.Clear();

        //we fill the list with empty objects
        for (int i = 0; i < numberWaypoints; i++)
        {
            lastPlacesSawPlayer.Add(null);
        }

        foreach (Transform waypoint in waypointsArray)
        {
            int i = 0;
            bool trier = false;
            while (i < numberWaypoints && !trier)
            {
                //if the place in the list is empty we put waypoint in it
                if (lastPlacesSawPlayer[i] == null)
                {
                    lastPlacesSawPlayer[i] = waypoint;
                    trier = true;
                }
                else
                {
                    //if the current waypoint is closest of position than the position in the list then our waypoint is put in the list at the i place 
                    if (Vector3.Distance(lastPlacesSawPlayer[i].position, position) > Vector3.Distance(waypoint.transform.position, position))
                    {
                        lastPlacesSawPlayer[i] = waypoint;
                        trier = true;
                    }
                }

                i++;
            }
        }
    }

    //CanSeePlayer: return true if monster can see the monster
    protected bool CanSeePlayer()
    {
        if (!isPlayerVisible) return false;

        bool res = false;
        Vector3 hauteur = new Vector3(0, 0, 0);
        RaycastHit hit;
        Vector3 rayDirection = Player.transform.position - (monsterHead.transform.position + hauteur);
        float localRangeOfView = rangeOfView;
        int layerMask = ~(1 << 2);

        foreach (InteractiveLight light in lightTab)
        {
            if (light.isInDark && !light.isActive && Vector3.Distance(light.transform.position, transform.position) < distanceToLight)
            {
                localRangeOfView = rangeOfViewInDark;
            }
        }

        //if player is in field of view
        if ((Vector3.Angle(rayDirection, Quaternion.Euler(0, 90, 0) * monsterHead.transform.forward)) <= fieldOfView * 0.5f)
        {

            if (Physics.Raycast(monsterHead.transform.position + hauteur, rayDirection, out hit, localRangeOfView, layerMask))
            {
                res = hit.transform.CompareTag("Player");

                //if the player is enough close to be seen by the monster
                if (res)
                {
                    lastTimeSeeingPlayer = Time.time;
                }
            }

        }

        // If the player is in hearing range
        if (Vector3.Distance(monsterHead.transform.position + hauteur, Player.transform.position) <= rangeOfHearing)
        {
            if (Physics.Raycast(monsterHead.transform.position + hauteur, rayDirection, out hit, rangeOfHearing, layerMask))
            {
                res = hit.transform.CompareTag("Player");

                // If there is no wall between the player and the monster
                if (res)
                {
                    lastTimeSeeingPlayer = Time.time;
                }
            }
        }

        if (sawPreviousFrame && fpsController.IsHidding)
        {
            playerSawInLocker = true;

        }

        if (res == true)
        {
            sawPreviousFrame = true;

            lastPlacePlayerSeen.transform.position = Player.transform.position;
            // Debug.Log(2);
        }
        else
        {
            if (!fpsController.IsHidding)
                sawPreviousFrame = false;
            // Debug.Log(3);
        }


        if (res == false && (fpsController.IsHidden || fpsController.IsHidding) && !playerSawInLocker)
        {
            // Debug.Log(4);
            lastTimeSeeingPlayer = 0;
            return false;
        }

        //if the player was seen the last 'timeToPurchase' seconds
        if (Time.time - lastTimeSeeingPlayer < timeToPurchase && lastTimeSeeingPlayer != 0)
        {
            //  Debug.Log(lastTimeSeeingPlayer);
            // Debug.Log(5);
            res = true;
        }



        return res;
    }



    //mustOpenDoor: return true if monster have a door in front of him
    protected bool mustOpenDoor()
    {
        //if we already opening a door stop the funciton
        if (doorToOpen != null && doorToOpen.Moving)
            return true;

        if (doorToOpen != null && doorToOpen.Locked)
        {
            doorToOpen = null;
            return false;
        }


        bool res = false;
        RaycastHit hit;
        Vector3 rayDirection = transform.forward;

        //we check doors for differents angles in front of the monster
        for (int i = -10; i <= 10; i += 10)
        {
            int layerMask = ~(1 << 2);
            if (Physics.Raycast(transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.Euler(0, i, 0) * rayDirection, out hit, 3f, layerMask))
            {
                //if the door is enough close to be seen by the monster
                CustomDoor door;
                GameObject hittedObject = hit.transform.gameObject;

                if (hittedObject.CompareTag("door"))
                {

                    while (hittedObject.transform.parent.gameObject.CompareTag("door"))
                    {
                        hittedObject = hittedObject.transform.parent.gameObject;
                    }

                    door = hittedObject.GetComponent<CustomDoor>();

                    if (door != null && !door.Locked && !door.Opened && !door.Moving)
                    {
                        res = true;
                        doorToOpen = door;
                    }
                }
            }
        }


        return res;
    }

    /*canAttack: if monster is enough close from the monster to be hit retunr true*/
    public bool canAttack()
    {

        if (Vector3.Distance(transform.position, Player.transform.position) <= rangeOfAttack) { return true; }
        else { return false; }
    }

    //choose waypoint randomly
    public void pickNewCurrentWaypoint()
    {
        Transform newWaypoint = waypointsArray[Random.Range(1, waypointsArray.Length)];
        while (newWaypoint == currentWaypoint)
        {
            newWaypoint = waypointsArray[Random.Range(1, waypointsArray.Length)];
        }
        currentWaypoint = newWaypoint;
    }

    //choose a new waypoint in function of lights and sounds
    public void pickDestination()
    {
        Transform newWaypoint = null;
        int i = 0;

        AudioSource[] sources = GameObject.FindObjectsOfType<AudioSource>();



        //if we want to go to lastPlacePlayerSeen we don't change the waypoint
        if (currentWaypoint == lastPlacePlayerSeen.transform && currentWaypoint != null)
        {
            return;
        }

        //if we have heard a song we go to the sound
        foreach (AudioSource source in sources)
        {
            if (source.isPlaying && source.gameObject.name != "DoorMain" && ((source.gameObject.layer == 17 && source.GetComponentInParent<ColliderTrigger>() != null && source.GetComponentInParent<ColliderTrigger>().canBeHeard) || (source.gameObject.tag == "door" && (doorToOpen == null || source.gameObject != doorToOpen.gameObject))))
            {
                if (source.gameObject.layer == 17)
                {
                    audioTarget.transform.position = source.transform.parent.transform.position;
                    audioTarget.transform.position = new Vector3(audioTarget.transform.position.x, 3, audioTarget.transform.position.z);
                }
                else
                {
                    foreach (Transform child in source.transform)
                    {
                        if (child.gameObject.name == "safePointExterior")
                        {
                            audioTarget.transform.position = child.gameObject.transform.position;
                        }
                    }

                    audioTarget.transform.position = new Vector3(audioTarget.transform.position.x, 1.9f, audioTarget.transform.position.z);

                }

                currentWaypoint = audioTarget.transform;

            }
        }

        //if we have heard a song we stop to check the lastPlacesSawPlayer
        if (currentWaypoint == audioTarget.transform)
        {
            lastPlacesSawPlayer.Clear();
            return;
        }



        //if we have not check all the lastPlacesSawPlayer we check them
        if (lastPlacesSawPlayer.Count > 0)
        {
            lastPlacePlayerSeen = lastPlacesSawPlayer[0].gameObject;
            currentWaypoint = lastPlacePlayerSeen.transform;
            lastPlacesSawPlayer.RemoveAt(0);
            return;
        }



        //if there are light on we go to light them off
        while (i < lightTab.Length)
        {
            if ((lightTab[i].isActive && newWaypoint == null) || (lightTab[i].isActive && Vector3.Distance(transform.position, newWaypoint.transform.position) > Vector3.Distance(transform.position, lightTab[i].transform.position)))
                newWaypoint = lightTab[i].transform;
            i++;
        }
        if (newWaypoint != null)
            currentWaypoint = newWaypoint;

        //else we choose a waypoint randomly
        else
        {
            if (currentWaypoint == null || currentWaypoint.GetComponent<InteractiveLight>())
            {
                pickNewCurrentWaypoint();
            }
        }
    }

    //check if the monster is at currentWaypoints
    public bool isAtWaypoint()
    {
        Vector3 destination = new Vector3(currentWaypoint.transform.position.x, transform.position.y, currentWaypoint.transform.position.z);
        if (currentWaypoint != null)
        {
            if (currentWaypoint.GetComponent<InteractiveLight>())
                return Vector3.Distance(transform.position, destination) < 2f;
            else
                return Vector3.Distance(transform.position, destination) < 0.5f;
        }
        else
            return false;
    }

    //check if the monster is at destination
    public bool isAtWaypoint(Vector3 destination)
    {
        destination = new Vector3(destination.x, transform.position.y, destination.z);

        if (currentWaypoint.GetComponent<InteractiveLight>())
            return Vector3.Distance(transform.position, destination) < 2f;
        else
            return Vector3.Distance(transform.position, destination) < 0.5f;
    }

    //check if the monster is at destination
    public bool isAtWaypoint(Vector3 destination, float delta)
    {
        destination = new Vector3(destination.x, transform.position.y, destination.z);


        return Vector3.Distance(transform.position, destination) < delta;

    }


    //update the music of the game depending of the distance beetwen the monster and the player
    public void updateMusic()
    {
        if (!stateMachineMonster.GetBool("canSeePlayer"))
        {
            //set the music in function of the distance to the player
            if (Vector3.Distance(transform.position, Player.transform.position) < farDistanceMusicMonsterClose)
            {
                if (!musicManager.canSetMusicMonsterClose())
                {
                    musicManager.setMusicMonsterCLose(1);
                }

                //update the music volume in function of distance to player
                musicManager.setTargetVolume((farDistanceMusicMonsterClose - Vector3.Distance(transform.position, Player.transform.position)) / farDistanceMusicMonsterClose);

            }
            else
            {
                if (!musicManager.canSetMusicMonsterFar())
                {
                    musicManager.setMusicMonsterFar(1);
                }

                //update the music volume in function of distance to player
                musicManager.setTargetVolume((Vector3.Distance(transform.position, Player.transform.position) - farDistanceMusicMonsterClose) / farDistanceMusicMonsterFar);
            }
        }
        else
        {
            if (!musicManager.canSetMusicMonsterChase())
            {
                musicManager.setMusicMonsterChase(1);
                musicManager.setTargetVolume(1);
            }
        }
    }


    public void lookAt(Vector3 destination, float time)
    {
        StartCoroutine(lookAtCoroutine(destination, time));
    }

    public IEnumerator lookAtCoroutine(Vector3 destination, float time)
    {
        float rotSpeed = 2;
        float timeStart = Time.time;

        if ((destination - transform.position).magnitude > 0.1f)
        {

            while (Time.time - timeStart < time)
            {
                Vector3 direction = (destination - transform.position).normalized;
                Quaternion qDir = Quaternion.LookRotation(direction);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, qDir, Time.deltaTime * rotSpeed);

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

}
