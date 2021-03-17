using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugClick : MonoBehaviour {
	public GameObject splat;
	public GameObject Bug;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown(){
		Destroy (Bug);
		splat.SetActive (true);
	} 
}
