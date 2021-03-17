using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triforce
{
    public class InteractiveItemTest : MonoBehaviour, IInteractable
    {
        public void OnHoverEnter()
        {
            Debug.Log("See object : " + gameObject.name);
        }

        public void OnHoverExit()
        {
            Debug.Log("Stop seeing object : " + gameObject.name);
        }

        public void OnInteract()
        {
            Debug.Log("You interact with " + gameObject.name);
        }
    }
}

