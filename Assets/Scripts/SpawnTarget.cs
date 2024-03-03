using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTarget : MonoBehaviour {
	private Vector3 min_bounds;
	private Vector3 max_bounds;
	private Collider2D myCollider;
	// Use this for initialization
	void Start () {
		myCollider = gameObject.GetComponent<Collider2D> ();
		max_bounds = myCollider.bounds.max;
		min_bounds = myCollider.bounds.min;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 getRandomPointInVolume(){
		float x, y;
		int attempts = 0;
		do {
			x = UnityEngine.Random.Range(min_bounds.x,max_bounds.x);
			y = UnityEngine.Random.Range(min_bounds.y,max_bounds.y);
			attempts++;
		} while(!this.myCollider.OverlapPoint (new Vector2 (x, y)));
		//Debug.Log ("Attempts : " + attempts);
		Vector3 spawnPoint = new Vector3 (x, y, 0);
		//Debug.Log ("Point : " + spawnPoint);
		return spawnPoint;
	}
}
