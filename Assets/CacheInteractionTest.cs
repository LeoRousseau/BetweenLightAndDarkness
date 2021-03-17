using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triforce
{
    public class CacheInteractionTest : MonoBehaviour, IInteractable
    {
        [SerializeField] CustomDoor m_Door;

        [SerializeField] Transform m_safePointExterior;
        [SerializeField] Transform m_safePointInterior;

        [SerializeField] Collider m_interactionCollider;

        private bool m_playerHidden;
        public bool PlayerHidden { get { return m_playerHidden; } }

        private FirstPersonController m_playerController;
        private CrossHairController m_crosshair;
        void Start()
        {
            m_playerController = FindObjectOfType<FirstPersonController>();
            m_crosshair = FindObjectOfType<CrossHairController>();
        }

        public void OnHoverEnter()
        {
            if (m_crosshair == null)
            {
                Debug.LogWarning("no crosshair controller found");
                return;
            }

            string message = m_playerHidden ? "Exit" : "Hide";
            m_crosshair.DoorMode(message);
        }

        public void OnHoverExit()
        {
            if (m_crosshair == null)
            {
                Debug.LogWarning("no crosshair controller found");
                return;
            }
            m_crosshair.StandardMode();
        }

        public void OnInteract()
        {
            if (!PlayerHidden && !m_Door.Locked && !m_playerController.IsHidden)
            {
                m_playerHidden = true;
                m_interactionCollider.enabled = false;

                m_playerController.EnterHideout();
                m_playerController.MovePlayerTo(m_safePointExterior, delegate{
                    if (!m_Door.Opened)
                    {
                        m_Door.OpenDoor(EnterInHideout);
                    }   
                }, true);
            }
            else if (PlayerHidden && m_playerController.IsHidden)
            {
                m_interactionCollider.enabled = false;
                m_Door.OpenDoor(ExitHideout);
            }          
        }

        void EnterInHideout()
        {
            m_playerController.MovePlayerTo(m_safePointInterior, delegate{
                m_Door.CloseDoor(delegate {
                    m_playerController.HideMode(m_Door.gameObject);
                    m_interactionCollider.enabled = true;
                });    
            }, true);
        }
        void ExitHideout()
        {
            m_safePointExterior.Rotate(new Vector3(0f, 180f, 0f));
            m_playerController.MovePlayerTo(m_safePointExterior, delegate
            {
                m_Door.CloseDoor();
                m_playerController.StopHide();

                m_interactionCollider.enabled = true;
                m_safePointExterior.Rotate(new Vector3(0f, -180f, 0f));
                m_playerHidden = false;
            }, true);
            

            
        }

        private IEnumerator WaitForDoorAnimation()
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

