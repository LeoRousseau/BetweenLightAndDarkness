using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Triforce
{
    public class PadLock : MonoBehaviour, IInteractable
    {
        public int code = 1035;
        private int actualRoulette;

        [Header("Roulettes")]
        public PadLockRoulette[] padLockRoulettes;

        [Header("Ui")]
        [SerializeField] GameObject ui;
        [SerializeField] float fadeSpeed;
        [SerializeField] float maxBlurSize;


        [Header("Interaction needs")]
        [SerializeField] CrossHairController m_crossHairController;
        [SerializeField] PostProcessProfile m_profile;
        [SerializeField] FirstPersonController m_playerController;
        [SerializeField] CustomDoor m_lockedDoor;
        [SerializeField] Collider m_padlockCollider;

        private Camera renderTextureCamera;
        private bool active = false;

        private bool m_isXAxisInUse = false;


        void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");
            renderTextureCamera = GetComponentInChildren<Camera>();

        }

        // Start is called before the first frame update
        void Start()
        {
            actualRoulette = 0;
            m_lockedDoor.LockDoor();
            desactivate();

        }

        void Update()
        {
            if (active)
            {
                if (Input.GetButtonDown("rightArrow") || ((Input.GetAxis("XArrowAxis") > 0.5f || Input.GetAxis("Horizontal") > 0.5f) && !m_isXAxisInUse))
                {
                    changeToRightRoulette();
                    m_isXAxisInUse = true;
                }
                else if (Input.GetButtonDown("leftArrow") || ((Input.GetAxis("XArrowAxis") < -0.5f || Input.GetAxis("Horizontal") < -0.5f) && !m_isXAxisInUse))
                {
                    changeToLeftRoulette();
                    m_isXAxisInUse = true;
                }
                else
                {

                    if (Input.GetButtonDown("Fire1"))
                    {

                        validateAnswer();
                    }
                    else if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Cancel"))
                    {
                        desactivate();
                    }
                    else if (Input.GetButtonDown("Unlock"))
                    {
                        unlock();
                    }
                }
                if (Mathf.Abs(Input.GetAxis("XArrowAxis")) < 0.001f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.001f)
                    m_isXAxisInUse = false;
            }
        }

        void OnDestroy()
        {
            ToggleBlurEffect(false);
        }

        private void changeToRightRoulette()
        {
            if (actualRoulette != padLockRoulettes.Length - 1)
            {
                padLockRoulettes[actualRoulette].desactiveRoulette();
                actualRoulette++;
                padLockRoulettes[actualRoulette].activateRoulette();
            }
        }

        private void changeToLeftRoulette()
        {
            if (actualRoulette != 0)
            {
                padLockRoulettes[actualRoulette].desactiveRoulette();
                actualRoulette--;
                padLockRoulettes[actualRoulette].activateRoulette();
            }
        }

        public void resetAllRoulette()
        {
            for (int p = 0; p < padLockRoulettes.Length; p++)
            {
                padLockRoulettes[p].resetValue();
            }
        }

        private bool checkAnswer()
        {
            bool resultTest = false;
            int somme = 0;
            for (int p = 0; p < padLockRoulettes.Length; p++)
            {
                somme += padLockRoulettes[p].getNumberValue() * ((int)Mathf.Pow((float)10, padLockRoulettes.Length - p - 1));
            }
            if (somme == code)
            {
                resultTest = true;
            }
            Debug.Log("Code tried = " + somme + " real code = " + code);

            return resultTest;
        }

        private int checkAnswerGame()
        {
            //bool resultTest = false;
            int somme = 0;
            for (int p = 0; p < padLockRoulettes.Length; p++)
            {
                somme += padLockRoulettes[p].getNumberValue() * ((int)Mathf.Pow((float)10, padLockRoulettes.Length - p - 1));
            }
            if (somme == code)
            {
                //resultTest = true;
            }
            Debug.Log("Code tried = " + somme + " real code = " + code);

            return somme;
        }

        private void generateRandomCode()
        {
            code = (int)Random.Range(1000f, 9999f);
        }
        public void validateAnswer()
        {
            int checkAnswerResult = checkAnswerGame();
            if (checkAnswerResult == code)
            {
                m_lockedDoor.UnlockDoor();
                desactivate();
                m_padlockCollider.enabled = false;
                Debug.Log("Door unlocked");
            }
            m_lockedDoor.OpenDoor();
        }

        private void unlock()
        {
            m_lockedDoor.UnlockDoor();
            desactivate();
            m_padlockCollider.enabled = false;
            m_lockedDoor.OpenDoor();
        }

        /*Implementation des fonctions de l'interface IInteractable*/
        public void OnHoverEnter()
        {
            if (m_crossHairController == null)
            {
                Debug.LogWarning("no crosshair controller found");
                return;
            }

            m_crossHairController.DoorMode("Unlock the padlock");
        }

        public void OnInteract()
        {
            activate();
            Debug.Log("Click sur le padlock detecte");
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

        private void ToggleBlurEffect(bool toggle)
        {
            Blur blurEffect = m_profile.GetSetting<Blur>();
            blurEffect.BlurSize.value = 0;
            blurEffect.active = toggle;
        }

        private void activate()
        {
            StartCoroutine(FadeUI(false, fadeSpeed));
            ui.SetActive(true);
            padLockRoulettes[actualRoulette].activateRoulette();
            renderTextureCamera.gameObject.SetActive(true);
            active = true;
            m_playerController.enabled = false;
            m_padlockCollider.enabled = false;
        }

        public void desactivate()
        {
            StartCoroutine(FadeUI(true, fadeSpeed));
            active = false;
            renderTextureCamera.gameObject.SetActive(false);
            padLockRoulettes[actualRoulette].desactiveRoulette();
            m_playerController.enabled = true;
            m_padlockCollider.enabled = true;
        }

        IEnumerator FadeUI(bool fadeAway, float fadeSpeed)
        {
            RawImage img = ui.GetComponent<RawImage>();
            Blur blurEffect = m_profile.GetSetting<Blur>();
            // fade from opaque to transparent
            if (fadeAway)
            {
                for (float i = 1; i >= 0; i -= Time.deltaTime * fadeSpeed)
                {
                    // set color with i as alpha
                    img.color = new Color(1, 1, 1, i);
                    blurEffect.BlurSize.value = i * maxBlurSize;
                    yield return null;
                }
                ui.SetActive(false);
                ToggleBlurEffect(false);
            }
            // fade from transparent to opaque
            else
            {
                ToggleBlurEffect(true);
                for (float i = 0; i <= 1; i += Time.deltaTime * fadeSpeed)
                {
                    // set color with i as alpha
                    img.color = new Color(1, 1, 1, i);
                    blurEffect.BlurSize.value = i * maxBlurSize;
                    yield return null;
                }
                ui.SetActive(true);
            }
        }
    }
}
