using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainText : MonoBehaviour {
	void Awake(){
		Text textComponent = gameObject.GetComponent<Text> ();
		textComponent.color = Globals.instance.textColor;
	}
}
