using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Utility;

namespace Triforce
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Speeds")]
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] private bool m_IsWalking;

        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;

        [SerializeField] private float m_GravityForce;
        [SerializeField] private MouseLook m_MouseLook;

        [Header("HeadBob")]
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();

        [Header("Sounds")]
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.


        private Vector2 m_Input;
        private CharacterController m_CharacterController;
        public CharacterController Character { get { return m_CharacterController; } }

        private Vector3 m_MoveDir = Vector3.zero;

        private CollisionFlags m_CollisionFlags;
        private Camera m_Camera;
        private Vector3 m_OriginalCameraPosition;

        private float m_StepCycle;
        private float m_NextStep;
        private AudioSource m_AudioSource;

        private GameObject m_hideout;
        public GameObject Hideout { get { return m_hideout; } }

        private bool AutomaticCameraMode { get; set; }
        private bool AutomaticMovesMode { get; set; }


        private bool m_isHidding;
        private bool m_isHidden;
        public bool IsHidding { get { return m_isHidding; } set { m_isHidding = value; } }
        public bool IsHidden { get { return m_isHidden; } set { m_isHidden = value; } }

        public void MouseLocked(bool val)
        {
            m_MouseLook.lockCursor = val;
            if (!val)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (!val);
        }

        void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_AudioSource = GetComponent<AudioSource>();

            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;

            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_MouseLook.Init(transform, m_Camera.transform);
        }


        private void FixedUpdate()
        {
            m_MouseLook.UpdateCursorLock();

            if (!AutomaticCameraMode)
            {
                RotateView(); // Orientation de la camera
            }

            if (!AutomaticMovesMode && m_CharacterController.enabled)
            {
                float speed;
                GetInput(out speed);

                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

                // get a normal for the surface that is being touched to move along it
                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                   m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;


                m_MoveDir.y = -m_GravityForce;


                m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

                ProgressStepCycle(speed);
                UpdateCameraPosition(speed);
            }
        }

        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            //m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
            m_IsWalking = !Input.GetButton("Run");
            if (m_IsWalking)
                m_IsWalking = Input.GetAxis("RunAxis") < 0.1f;
        }

        private void UpdateCameraPosition(float speed)
        {
            if (!m_UseHeadBob)
            {
                return;
            }
            Vector3 newCameraPosition;
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }

        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }

        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }

        private void PlayFootStepAudio()
        {
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = UnityEngine.Random.Range(1, m_FootstepSounds.Length);

            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        public void MovePlayerTo(Transform target, Action callback = null, bool rotate = false)
        {
            if (rotate)
            {
                StartCoroutine(ForcePlayerToReachTransformAndRotate(target, callback));
            }
            else
            {
                StartCoroutine(ForcePlayerToReachTransform(target, callback));
            }
        }

        private IEnumerator ForcePlayerToReachTransform(Transform target, Action callback = null)
        {
            AutomaticMovesMode = true;
            Vector3 offset;
            do
            {
                offset = target.position - transform.position;
                //Ignore Y axis
                offset.y = 0;



                m_CharacterController.Move((offset.normalized * m_WalkSpeed) * Time.fixedDeltaTime);
                yield return Time.fixedDeltaTime;

            } while (offset.magnitude > 0.1f && AutomaticMovesMode);

            AutomaticMovesMode = false;

            if (callback != null)
            {
                callback.Invoke();
            }
        }
        private IEnumerator ForcePlayerToReachTransformAndRotate(Transform target, Action callback = null)
        {
            AutomaticMovesMode = true;
            AutomaticCameraMode = true;

            float timeCountRot = 0.0f;
            float timeCountPos = 0.0f;
            Quaternion originRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            Vector3 originPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z); //On ignore la hauteur?


            float rotFactor = 0.05f;
            float transFactor = 0.03f;
            do
            {
                if ((timeCountRot < 1 && AutomaticCameraMode))
                {
                    transform.rotation = Quaternion.Slerp(originRot, target.rotation, timeCountRot);
                    timeCountRot += Time.deltaTime + timeCountRot * rotFactor;
                    ProgressStepCycle(m_RunSpeed);
                }


                if ((timeCountPos < 1 && AutomaticMovesMode))
                {
                    transform.position = Vector3.Lerp(originPos, targetPos, timeCountPos);
                    timeCountPos += Time.deltaTime + timeCountPos * transFactor;
                }

                yield return null;

            } while ((timeCountPos < 1 && AutomaticMovesMode) || (timeCountRot < 1 && AutomaticCameraMode));

            m_MouseLook.Init(transform, m_Camera.transform);
            AutomaticMovesMode = false;
            AutomaticCameraMode = false;

            if (callback != null)
            {
                callback.Invoke();
            }
        }

        private void FullControlMode(bool enabled, bool colliderActive = true)
        {
            AutomaticMovesMode = enabled;
            AutomaticCameraMode = enabled;
            m_CharacterController.enabled = colliderActive;

        }

        public void EnterHideout()
        {
            Debug.Log("isHidding");
            m_isHidding = true;
            FullControlMode(true, false);
        }
        public void HideMode(GameObject hideout)
        {
            m_isHidding = false;
            m_isHidden = true;
            m_hideout = hideout;

            AutomaticMovesMode = true;

            //m_MouseLook.clampHorizontalRotation = true;
            m_MouseLook.AddYRotation(hideout.transform.rotation.eulerAngles.y);

            m_forceAvailable = true;
        }
        public void StopHide()
        {
            m_isHidden = false;

            m_MouseLook.clampHorizontalRotation = false;
            FullControlMode(false, true);

            m_hideout = null;

            m_forceAvailable = false;
        }


        bool m_forceAvailable = false;
        public bool IsAvailable()
        {
            return (!AutomaticMovesMode && !AutomaticCameraMode && !IsHidding) || m_forceAvailable;
        }

    }
}

