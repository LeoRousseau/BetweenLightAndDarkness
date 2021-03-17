using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugRegionSpawner : MonoBehaviour {

	public Vector2 xRegionSize 	= new Vector2(-1f,1f);
	public Vector2 yRegionSize 	= new Vector2(-1f,1f);

	public Vector2 spawnTimerRange 		= new Vector2(2f,3f);
	public Vector2 spawnAmmountRange 	= new Vector2(5,10);

	public GameObject Bug;
	public bool autoSpawn = true;

	void Start ()
	{
		StartCoroutine (SpawnCooker ());
	}

	void Spawn (int ammount) 
	{         

		for (int i =0; i < ammount; i++){

			Instantiate (Bug, transform);;


		}

	}

	IEnumerator SpawnCooker() 
	{
		yield return new WaitForSeconds(Random.Range(spawnTimerRange.x, spawnTimerRange.y));
		Spawn((int)Random.Range(spawnAmmountRange.x, spawnAmmountRange.y));
	}
}
