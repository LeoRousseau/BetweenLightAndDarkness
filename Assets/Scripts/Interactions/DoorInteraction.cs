using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Triforce
{
    public class DoorInteraction : MonoBehaviour, IInteractable
    {
        [Header("Controllers")]
        [SerializeField] protected CustomDoor m_Door;

        //Should be passed as parameter in IInteractable fonctions
        protected FirstPersonController m_playerController;
        protected CrossHairController m_crossHairController;

        [Header("Key points")]
        [SerializeField] protected Collider m_interactionCollider;
        [SerializeField] protected Transform m_safePointExterior;
        [SerializeField] protected Transform m_safePointInterior;
        [SerializeField] protected Collider m_nearZone;

        protected virtual void Start()
        {
            if (m_Door == null)
            {
                m_Door = GetComponentInParent<CustomDoor>();
            }
            m_interactionCollider = this.GetComponent<Collider>();

            m_crossHairController = FindObjectOfType<CrossHairController>();
            m_playerController = FindObjectOfType<FirstPersonController>();
        }

        public virtual void OnInteract()
        {
            if (m_Door.Moving)
                return;
       
            if (m_Door.Opened)
            {
                //float angle = Vector3.Angle(m_playerController.transform.forward, this.transform.right);
                float angle = Vector3.SignedAngle(m_playerController.transform.forward, this.transform.right, Vector3.up);

                //Debug.Log(Vector3.SignedAngle(m_playerController.transform.forward, this.transform.right, Vector3.up));
                if (m_playerController.Character.bounds.Intersects(m_nearZone.bounds))
                {
                   /* if (0f < angle && angle < 90)
                    {
                        m_playerController.MovePlayerTo(m_safePointExterior);
                    }
                    else if (90f <= angle && angle <= 180f)
                    {
                        m_playerController.MovePlayerTo(m_safePointInterior);
                    }*/
                }


                m_Door.CloseDoor();
                StartCoroutine(WaitForDoorAnimation());
                
            }
            else if (!m_Door.Opened)
            {
                if (m_playerController.Character.bounds.Intersects(m_nearZone.bounds))
                {
                    m_playerController.MovePlayerTo(m_safePointExterior);
                }
                m_Door.OpenDoor();
                StartCoroutine(WaitForDoorAnimation());
            }
        }

        public virtual void OnHoverEnter()
        {
            if (m_crossHairController == null)
            {
                Debug.LogWarning("no crosshair controller found");
                return;
            }
            string message;
            if (m_Door.Moving)
            {
                message = "";
            }
            else 
            {
                message = m_Door.Opened ? "Close door" : "Open door";
            }


            m_crossHairController.DoorMode(message);
        }
        public void OnHoverExit()
        {
            if (m_crossHairController == null)
            {
                Debug.LogWarning("no crosshair controller found");
                return;
            }
            m_crossHairController.StandardMode();
        }

        private float GetAngleBetweenController()
        {
            return Vector3.Angle(m_playerController.transform.forward, this.transform.right);
        }

        protected IEnumerator WaitForDoorAnimation()
        {
            m_interactionCollider.enabled = false;
            while (m_Door.Moving)
            {
                yield return null;
            }

           m_interactionCollider.enabled = true;
        }
    }
}

