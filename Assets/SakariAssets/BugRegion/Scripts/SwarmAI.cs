using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmAI : MonoBehaviour {

    public BugsAI[] Bugs;
    BugTargetStruct[] Targets;
    public Vector2 TargetIntervalRange = new Vector2(0.1f, 0.2f);
    public Vector2 xRegionSize = new Vector2(-1f, 1f);
    public Vector2 yRegionSize = new Vector2(-1f, 1f);

    // Use this for initialization
    void Awake () {
        Targets = new BugTargetStruct[Bugs.Length];
		for(int i = 0; i < Bugs.Length; i++)
        {
            Targets[i] = new BugTargetStruct();
            Targets[i].Trans = new GameObject("BugTarget" + (i + 1)).transform;
            Targets[i].Trans.parent = transform;
            Targets[i].Interval = Random.Range(TargetIntervalRange.x,TargetIntervalRange.y);
            Targets[i].tick = 0;
           Bugs[i].BugTarget = Targets[i].Trans;
        }
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < Targets.Length; i++)
        {
            Targets[i].tick += Time.deltaTime;
            if (Targets[i].tick > Targets[i].Interval)
            {
                Targets[i].Trans.localPosition = new Vector3(Random.Range(xRegionSize.x, xRegionSize.y), 0, Random.Range(yRegionSize.x, yRegionSize.y));
                Targets[i].tick = 0;                
            }
        }
	}
}
public struct BugTargetStruct
{
    public Transform Trans;
    public float Interval;
    public float tick;
}