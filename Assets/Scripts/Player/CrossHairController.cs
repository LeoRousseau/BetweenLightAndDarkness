using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Triforce
{
    public class CrossHairController : MonoBehaviour
    {
        [Header("Cursor Images")]
        [SerializeField] Image m_standardCursor;
        [SerializeField] Image m_doorCursor;

        [Header("Messages")]
        [SerializeField] Text m_message;


        private Image m_currentCursor;

        void Start()
        {
            m_currentCursor = m_standardCursor;
            DisableAllCursors();
            StandardMode();
        }


        public void StandardMode()
        {
            SetCursor(m_standardCursor, "");
        }
        public void DoorMode(string message = "")
        {
            SetCursor(m_doorCursor, message);
        }

        public void SetCursor(Image selectedCursor, string message = "")
        {
            m_currentCursor.gameObject.SetActive(false);

            m_currentCursor = selectedCursor;
            m_currentCursor.gameObject.SetActive(true);

            m_message.text = message;
        }

        private void DisableAllCursors()
        {
            m_standardCursor.gameObject.SetActive(false);
            m_doorCursor.gameObject.SetActive(false);
        }
    }
}

