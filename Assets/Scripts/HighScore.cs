using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
	public Globals.GameModes gameMode;
	// Use this for initialization
	void Start () {
		Text textComponent = gameObject.GetComponent<Text> ();
		textComponent.text = "(" + Personalization.instance.getHighScore (gameMode).ToString() + ")";
	}
}
