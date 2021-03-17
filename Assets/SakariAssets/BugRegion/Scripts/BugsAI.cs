using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugsAI : MonoBehaviour {

	public Vector2 SpeedRange 	= new Vector2(1f,3f);
    public Vector2 ScaleRange = new Vector2(1f, 3f);
    public Vector2 RunRange 	= new Vector2(1f,5f);
	public Vector2 WaitRange 	= new Vector2(1f,5f);

	public float smoothing = 1f;
	public float rotSpeed = 3f;

	public Transform BugTarget;

	float speed;
	Vector3 targetpos;
    float bugscale;
	float waittime = 1f;
	float RunTime = 1f;
	private bool Sleep = false;

	void Start () {
        speed = Random.Range(SpeedRange.x, SpeedRange.y);
        RunTime = Random.Range(RunRange.x, RunRange.y);
        waittime = Random.Range(WaitRange.x, WaitRange.y);
        bugscale = Random.Range(ScaleRange.x, ScaleRange.y);
        StartCoroutine(Alive());
		//StartCoroutine (FollowTarget (BugTarget));
		transform.localScale = new Vector3(bugscale, bugscale, bugscale);

	}
		
		
	IEnumerator Alive ()
	{
		while (true)
		{
			Sleep = false;
            waittime = Random.Range(WaitRange.x, WaitRange.y);
            yield return new WaitForSeconds(RunTime);

			Sleep = true;
            RunTime = Random.Range(RunRange.x, RunRange.y);
            yield return new WaitForSeconds(waittime);
		}
	}
		
		
	void Update()
	{
        if (Sleep)
            return;
		transform.Translate(Vector3.forward * speed * 1.5f * Time.deltaTime, Space.Self);
		Vector3 D = BugTarget.transform.localPosition - transform.localPosition;  

		// calculate the Quaternion for the rotation
		Quaternion rot = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(D), rotSpeed * Time.deltaTime);

		//Apply the rotation 
		transform.localRotation = rot; 

		// put 0 on the axys you do not want for the rotation object to rotate
		transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); 		
	}
}

