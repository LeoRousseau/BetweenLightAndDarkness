using UnityEngine;

namespace Triforce
{
    public class InteractiveLight : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject m_lightEffect;
        [SerializeField] CrossHairController m_crossHairController;
        [SerializeField] MeshRenderer m_candleMesh;
        [SerializeField] Material m_candleLit;
        [SerializeField] Material m_candleUnlit;
        [SerializeField] bool m_IsOnAtStart;
        [SerializeField] AudioClip m_soundOn;
        [SerializeField] AudioClip m_soundOff;
        [SerializeField] ParticleSystem m_smoke;

        public bool isActive = false;

        public bool isInDark = false;

        AudioSource SoundFX;

        void Start()
        {
            m_crossHairController = FindObjectOfType<CrossHairController>();
            SoundFX = GetComponent<AudioSource>();
            if (m_lightEffect == null)
            {
                Debug.Log("missing gameobject for lightEffect");
            }

            if (m_IsOnAtStart)
            {
                TurnOn(true);
            }
            else
            {
                TurnOff(true);
            }
        }

        public void OnInteract()
        {
            bool isActive = m_lightEffect.activeSelf;
            if (isActive)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }

            string message;
            message = m_lightEffect.activeSelf ? "Blow out" : "Light up";
            m_crossHairController.DoorMode(message);
        }

        public void OnHoverEnter()
        {
            if (m_crossHairController == null)
            {
                Debug.LogWarning("no crosshair controller found");
                return;
            }
            string message;
            message = m_lightEffect.activeSelf ? "Blow out" : "Light up";
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

        public void TurnOn(bool init = false)
        {
            if (!init)
            {
                string message;
                message = "Blow out";
                SoundFX.clip = m_soundOn;
                SoundFX.Play();
                m_crossHairController.DoorMode(message);
            }
            m_candleMesh.material = m_candleLit;
            m_lightEffect.SetActive(true);
            isActive = true;
        }

        public void TurnOff(bool init = false)
        {
            if (!init)
            {
                SoundFX.clip = m_soundOff;
                SoundFX.Play();

                m_smoke.Play();
            }
            m_lightEffect.SetActive(false);
            m_candleMesh.material = m_candleUnlit;
            isActive = false;
        }
    }
}