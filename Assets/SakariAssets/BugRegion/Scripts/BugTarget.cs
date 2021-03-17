using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugTarget : MonoBehaviour {

	public Vector2 TargetIntervalRange 	= new Vector2(0.1f,0.2f);

	public float smoothing = 1f;
	public float speed;

	Vector3 targetpos;
	float x;
	float z;
	float TargetInterval;

	public Vector2 xRegionSize 	= new Vector2(-1f,1f);
	public Vector2 yRegionSize 	= new Vector2(-1f,1f);

	void Awake ()
	{
		TargetInterval = Random.Range (TargetIntervalRange.x, TargetIntervalRange.y);

	}

	void Start () {
		StartCoroutine(RandomTargetLocation());

	}

	void Update () {
		placenewtarget ();

	}
		

	void placenewtarget ()
	{

		transform.localPosition = new Vector3(x, 0, z);
	}

	IEnumerator RandomTargetLocation ()
	{
		while (true) {
			x = Random.Range(xRegionSize.x,xRegionSize.y);
			z = Random.Range(yRegionSize.x,yRegionSize.y);
			yield return new WaitForSeconds(TargetInterval);
		}
	}
		
}

