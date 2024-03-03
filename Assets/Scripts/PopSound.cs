using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopSound : MonoBehaviour {
	void Awake () {
		if (Globals.UI.gameOverScreen.activeSelf) {
			gameObject.GetComponent<AudioSource> ().pitch = 1 + Random.Range (-0.05f, 0.05f);
			float delay = Random.Range (0f, 0.1f);
			gameObject.GetComponent<AudioSource> ().PlayDelayed (delay);
		} else {
			gameObject.GetComponent<AudioSource> ().PlayOneShot (gameObject.GetComponent<AudioSource> ().clip);
		}
	}
}
