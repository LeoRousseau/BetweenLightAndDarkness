using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour {

    
    bool isEnabled;
    
    [SerializeField]
    float flySpeed = 0.5f;
    [SerializeField]
    float accelerationRatio = 1;
    [SerializeField]
    float slowDownRatio = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            flySpeed *= accelerationRatio;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            flySpeed /= accelerationRatio;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            flySpeed *= slowDownRatio;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            flySpeed /= slowDownRatio;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(transform.forward * flySpeed * Input.GetAxis("Vertical"), Space.World);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(transform.right * flySpeed * Input.GetAxis("Horizontal"), Space.World);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.up * flySpeed * 0.5f);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(-Vector3.up * flySpeed * 0.5f);
        }
    }
}
