using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberedSpawnObject : SpawnObject {
	private int selfNumber;
	public void setNumber(int number){
		TextMesh textmesh = gameObject.GetComponentInChildren<TextMesh> ();
		textmesh.text = number.ToString ();
		selfNumber = number;
	}

	public int getNumber(){
		return selfNumber;
	}

	/*public override void destroy(bool updateScore = true){
		if (!Globals.UI.gameOverScreen.activeSelf) {
			NumberedSpawnObject[] otherObjects = GameObject.FindObjectsOfType<NumberedSpawnObject> ();
			foreach (NumberedSpawnObject obj in otherObjects) {
				if (obj != this && selfNumber > obj.getNumber ()) {
					Globals.UI.showGameOverScreen ();
					updateScore = false;
					break;
				}
			}
		}
		base.destroy (updateScore);
	}*/
}
