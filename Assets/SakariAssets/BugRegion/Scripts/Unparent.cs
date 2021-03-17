using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unparent : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.parent = null;
    }


}
