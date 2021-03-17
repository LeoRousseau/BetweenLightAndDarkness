using UnityEngine;

namespace Triforce
{
    public class PadLockRoulette : MonoBehaviour
    {
        private int value;
        private bool activeRoulette;


        public float rouletteRotationSpeed;
        private Quaternion destinationAngle;
        private Outline outline;
        private AudioSource audioSource;

        private void Awake()
        {
            outline = GetComponent<Outline>();
            audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            value = 0;
            outline.enabled = false;
            activeRoulette = false;
            destinationAngle = transform.rotation;
        }


        // Update is called once per frame
        void Update()
        {
            float axisButton = Input.GetAxisRaw("Vertical");
            if (axisButton == 0)
            {
                axisButton = Input.GetAxisRaw("YArrowAxis");
            }
            if (axisButton == -1)
            {
                valueDown();
            }
            else if (axisButton == 1)
            {
                valueUp();
            }

            if (transform.rotation != destinationAngle)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, destinationAngle, rouletteRotationSpeed * Time.deltaTime);
            }

        }

        public void valueUp()
        {
            if (activeRoulette && transform.rotation == destinationAngle)
            {
                if (value == 0)
                {
                    value = 9;
                }
                else
                {
                    value--;
                }
                destinationAngle = transform.rotation * Quaternion.Euler(36, 0, 0);
                audioSource.Play();
            }
        }

        public void valueDown()
        {
            if (activeRoulette && transform.rotation == destinationAngle)
            {
                if (value == 9)
                {
                    value = 0;
                }
                else
                {
                    value++;
                }
                destinationAngle = transform.rotation * Quaternion.Euler(-36, 0, 0);
                audioSource.Play();
            }
        }

        public void resetValue()
        {
            value = 0;
        }

        public int getNumberValue()
        {
            return value;
        }

        public bool getActiveRoulette()
        {
            return activeRoulette;
        }

        public void setActiveRoulette(bool b)
        {
            activeRoulette = b;
            outline.enabled = b;
        }


        public void activateRoulette()
        {
            setActiveRoulette(true);
        }

        public void desactiveRoulette()
        {
            setActiveRoulette(false);
        }


    }
}
