using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triforce
{
    public interface IInteractable
    {
        void OnHoverEnter();
        void OnInteract();
        void OnHoverExit();
    }

}
