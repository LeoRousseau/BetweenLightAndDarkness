using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Triforce
{
    public class InteractionController : MonoBehaviour
    {
        private IInteractable m_raycastedObject;
        private IInteractable m_previousObject;
        private GameManager m_gameManager;
        private FirstPersonController m_playerController;

        [Range(0,5)]
        [SerializeField] float m_DetectionLength;

        [SerializeField] string[] m_InteractiveLayers;

        private bool m_wasHovering;


        private Camera m_MainCamera;

        private void Start()
        {
            m_MainCamera = Camera.main;
            m_gameManager = GameObject.FindObjectOfType<GameManager>();
            m_playerController = FindObjectOfType<FirstPersonController>();
        }

        void Update()
        {
            if (m_gameManager==null)
            {
                m_gameManager = GameObject.FindObjectOfType<GameManager>();
            }
            if (m_gameManager.allowInteractions)
            {
                SelectItemFromRay();

                if (m_raycastedObject != null)
                {

                    if (IsInterracting())
                    {
                        m_raycastedObject.OnInteract();
                    }

                    if (m_previousObject == null)
                    {
                        m_raycastedObject.OnHoverEnter();
                        m_previousObject = m_raycastedObject;
                    }
                    else if (!m_previousObject.Equals(m_raycastedObject))
                    {
                        m_previousObject.OnHoverExit();
                        m_raycastedObject.OnHoverEnter();
                        m_previousObject = m_raycastedObject;
                    }
                }
                else
                {
                    if (m_previousObject != null)
                    {
                        m_previousObject.OnHoverExit();
                        m_previousObject = null;
                    }
                }
            }
        }

        private void SelectItemFromRay()
        {
            if (!m_playerController.IsAvailable())
            {
                m_raycastedObject = null;
                return;
            }
            // Le raycast mesure plus long en horizontal pour détecter les choses qui sont par terre
            // Il ne détecte que ce qui est sur les layers spécifiés
            
            Ray ray = m_MainCamera.ViewportPointToRay( Vector3.one / 2f );
            RaycastHit hitInfo;

            float horizonLength = Mathf.Clamp(new Vector2(ray.direction.x, ray.direction.z).magnitude, 0.01f, 1.0f);
            float rayLength = m_DetectionLength / horizonLength;

            if(Physics.Raycast(ray, out hitInfo, rayLength, LayerMask.GetMask(m_InteractiveLayers)))
            {
                m_raycastedObject = hitInfo.collider.gameObject.GetComponent<IInteractable>();
                Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green, 0, false);
            }
            else
            {
                m_raycastedObject = null;
                Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 0, false);
            }
        }

        private bool IsInterracting()
        {
            return Input.GetButtonDown("Interact") && m_playerController.IsAvailable();
        }
    }
}

