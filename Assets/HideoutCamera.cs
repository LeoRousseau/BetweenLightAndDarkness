using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class HideoutCamera : MonoBehaviour
{
    [SerializeField] private MouseLook m_MouseLook;

    public Camera HideCamera;
    public Transform HidePoint;
    void Start()
    {
        m_MouseLook.Init(transform, HideCamera.transform);
        m_MouseLook.UpdateCursorLock();
    }

    void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
        m_MouseLook.LookRotation(transform, HideCamera.transform);
    }
}
