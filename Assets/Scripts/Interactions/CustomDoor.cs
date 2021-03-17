using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Triforce
{
    #region Classe Utilitaires
    [Serializable]
    public class DoorControls
    {
        public float openingSpeed = 1;
        public float closingSpeed = 1.3f;
        [Range(0, 1)]
        public float closeStartFrom = 0.6f;

        [Range(0, 1)]
        public float OpenedAtStart = 0; //if grather than 0, the door will be in open state at start.
    }
    [Serializable]
    public class DoorSounds
    {
        public bool enabled = true;
        public AudioClip open;
        public AudioClip close;
        public AudioClip locked;
        public AudioClip unlock;
        [Range(0, 1.0f)]
        public float volume = 1.0f;
        [Range(0, 0.4f)]
        public float pitchRandom = 0.2f;
    }
    [Serializable]
    public class AnimNames //names of the animations, which you use for every action
    {
        public string OpeningAnim = "Door_open";
        public string LockedAnim = "Door_locked";
    }
    #endregion

    public class CustomDoor : MonoBehaviour
    {
        #region Attributs
        [SerializeField] DoorControls controls = new DoorControls();
        [SerializeField] DoorSounds doorSounds = new DoorSounds();
        [SerializeField] AnimNames AnimationNames = new AnimNames();

        [SerializeField] bool m_isLocked = false;
        public bool Locked { get { return m_isLocked; } set { m_isLocked = value; } }

        [SerializeField] bool m_isHideout = false;


        Animation doorAnimation;
        AudioSource SoundFX;

        public Transform waypoint;
        public NavMeshObstacle navMeshObstacle;


        bool m_isOpen;
        public bool Opened { get { return m_isOpen; } set { m_isOpen = value; } }

        bool m_isInMovment;
        public bool Moving { get { return m_isInMovment; } }

        public GameObject inventory;
        public Item key;
        #endregion


        // Start is called before the first frame update
        void Start()
        {
            tag = m_isHideout ? "hideout" : "door";
            foreach (Transform t in transform)
            {
                t.gameObject.tag = tag;
                foreach (Transform encoreMdr in t)
                {
                    encoreMdr.gameObject.tag = tag;
                    foreach (Transform aCorriger in encoreMdr)
                    {
                        aCorriger.gameObject.tag = tag;
                    }
                }
                if (t.gameObject.name == "DetectionZone")
                {
                    t.gameObject.layer = 2;
                }
            }

            if (Locked)
            {
                if (waypoint != null)
                    waypoint.gameObject.SetActive(false);
                navMeshObstacle.enabled = true;
            }
            else
            {
                if (waypoint != null)
                    waypoint.gameObject.SetActive(true);
                if (Opened)
                    navMeshObstacle.enabled = true;
                else
                    navMeshObstacle.enabled = false;
            }

            doorAnimation = GetComponent<Animation>();
            SoundFX = GetComponent<AudioSource>();

            if (controls.OpenedAtStart > 0)
            {
                doorAnimation[AnimationNames.OpeningAnim].normalizedTime = controls.OpenedAtStart;
                doorAnimation[AnimationNames.OpeningAnim].speed = 0;
                doorAnimation.Play(AnimationNames.OpeningAnim);

                m_isOpen = true;
            }
        }

        public void LockDoor()
        {
            Locked = true;
            if (waypoint != null)
                waypoint.gameObject.SetActive(false);
            navMeshObstacle.enabled = true;
        }

        public void UnlockDoor()
        {
            PlaySFX(doorSounds.unlock);
            Locked = false;
            if (waypoint != null)
                waypoint.gameObject.SetActive(true);
            if (Opened)
                navMeshObstacle.enabled = true;
            else
                navMeshObstacle.enabled = false;
        }

        public void OpenDoor(Action callback = null)
        {

            if (m_isInMovment || m_isOpen)
                return;

            if (Locked)
            {
                OpenLockedDoor();
                return;
            }
            //Start animation
            doorAnimation[AnimationNames.OpeningAnim].speed = controls.openingSpeed;
            doorAnimation[AnimationNames.OpeningAnim].normalizedTime = doorAnimation[AnimationNames.OpeningAnim].normalizedTime;
            doorAnimation.Play(AnimationNames.OpeningAnim);

            //Start Coroutine animation
            StartCoroutine(WaitForOpenedDoor(doorAnimation, callback));
        }
        private IEnumerator WaitForOpenedDoor(Animation animation, Action callback = null)
        {
            m_isInMovment = true;
            PlaySFX(doorSounds.open);
            navMeshObstacle.enabled = true;

            while (animation.isPlaying)
            {
                yield return null;
            }

            Opened = true;
            m_isInMovment = false;

            if (callback != null)
            {
                callback.Invoke();
            }
        }

        public void TriggerCloseDoor()
        {
            if (m_isOpen)
                CloseDoor(null);
        }

        public void CloseDoor(Action callback = null)
        {
            //A quoi sert cette ligne? Dependance tres lourde!!! (impossible de mettre une porte sans l'ia dans une scene....)

            if (!m_isHideout && GameObject.FindObjectOfType<StateMachineMonsterController>() != null && GameObject.FindObjectOfType<StateMachineMonsterController>().stateMachineMonster.GetBool("canSeePlayer"))
                return;

            if (m_isInMovment)
                return;

            //Start Closing animation
            doorAnimation[AnimationNames.OpeningAnim].speed = -controls.closingSpeed;
            doorAnimation[AnimationNames.OpeningAnim].normalizedTime = controls.closeStartFrom;
            doorAnimation.Play(AnimationNames.OpeningAnim);

            //Start Coroutine animation
            StartCoroutine(WaitForClosedDoor(doorAnimation, callback));
        }
        private IEnumerator WaitForClosedDoor(Animation animation, Action callback = null)
        {
            m_isInMovment = true;

            while (animation.isPlaying)
            {
                yield return null;
            }

            PlaySFX(doorSounds.close);
            navMeshObstacle.enabled = false;

            Opened = false;
            m_isInMovment = false;

            if (callback != null)
            {
                callback.Invoke();
            }
        }

        public void OpenLockedDoor()
        {
            //to do : isolate and play knob animation

            if (key != null)
            {
                if (key.pickedUp)
                {
                    inventory.GetComponent<Inventory>().Use(key);
                    UnlockDoor();
                    PlaySFX(doorSounds.unlock);
                    OpenDoor();
                }
                else
                {
                    PlaySFX(doorSounds.locked);
                }

            }
            else
            {
                PlaySFX(doorSounds.locked);
            }
        }

        #region SFX
        void PlaySFX(AudioClip clip, float delay = 0f)
        {
            if (!doorSounds.enabled)
                return;

            SoundFX.pitch = UnityEngine.Random.Range(1 - doorSounds.pitchRandom, 1 + doorSounds.pitchRandom);
            SoundFX.clip = clip;
            if (delay > 0)
            {
                SoundFX.PlayDelayed(delay);
            }
            else
            {
                SoundFX.Play();
            }
        }
        #endregion
    }
}

