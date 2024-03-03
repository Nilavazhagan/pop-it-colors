using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSpawnVolume : MonoBehaviour {
	public Vector2[] Multipliers;
	public Vector2[] Adders;
	private float screenWidth;
	private float screenHeight;
	// Use this for initialization
	void Start () {
		updatePositions ();
	}
	
	// Update is called once per frame
	void Update () {
		if (screenWidth != Screen.width || screenHeight != Screen.height)
			updatePositions ();
	}

	void updatePositions(){
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		Vector2 screenBounds = Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth, screenHeight));
		Vector2 screenBoundary = new Vector2 (Mathf.Ceil (screenBounds.x), Mathf.Ceil (screenBounds.y));
		if (screenBounds.x - screenBoundary.x < 0.5f) {
			screenBoundary.x = screenBounds.x + 0.5f;
		}
		if (screenBounds.y - screenBoundary.y < 0.5f) {
			screenBoundary.y = screenBounds.y + 0.5f;
		}
		Vector2[] points = new Vector2[Multipliers.Length];
		PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D> ();
		for (int i = 0; i < Multipliers.Length; i++) {
			points [i] = Vector2.Scale (Multipliers [i], screenBoundary) + Adders [i];
		}
		collider.SetPath (0, points);
	}
}
