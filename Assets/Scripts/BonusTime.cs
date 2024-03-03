using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusTime : MonoBehaviour {
	float initialYPos, finalYPos;
	float initialOpacity = 1f, finalOpacity = 0f;
	float lerp = 0f;
	Vector2 localPosition;
	Color color;
	// Use this for initialization
	void Start () {
		localPosition = gameObject.GetComponent<RectTransform> ().localPosition;
		color = gameObject.GetComponent<Text> ().color;
		initialYPos = localPosition.y;
		finalYPos = initialYPos + 5f;
	}
	
	// Update is called once per frame
	void Update () {
		lerp += Time.deltaTime;
		float yPos = Mathf.Lerp (initialYPos, finalYPos, lerp);
		float opacity = Mathf.Lerp (initialOpacity, finalOpacity, lerp);
		localPosition.y = yPos;
		gameObject.GetComponent<RectTransform> ().localPosition = localPosition;
		color.a = opacity;
		gameObject.GetComponent<Text> ().color = color;
		if (opacity == finalOpacity && yPos == finalYPos) {
			Destroy (this.gameObject);
		}
	}

}
