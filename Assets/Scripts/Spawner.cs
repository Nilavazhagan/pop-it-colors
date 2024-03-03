using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : SpawnTarget {
	public SpawnTarget spawnTarget;
	public SpawnObject spawnObject;
	public NumberedSpawnObject numberedSpawnObject;

	public void spawnADot(float speed){
		Vector3 spawnPos = this.getRandomPointInVolume ();
		SpawnObject spawnedObject = Instantiate (spawnObject, spawnPos, new Quaternion());
		setColorAndEject (spawnedObject, speed);
	}

	public void spawnADot(float speed, int dotNumber){
		if (Globals.instance.currentGameMode == Globals.GameModes.NUMBERS) {
			Vector3 spawnPos = this.getRandomPointInVolume ();
			NumberedSpawnObject spawnedObject = Instantiate (numberedSpawnObject, spawnPos, new Quaternion ());
			spawnedObject.setNumber (dotNumber);
			setColorAndEject ((SpawnObject)spawnedObject, speed);
		} else {
			spawnADot (speed);
		}
	}

	private void setColorAndEject(SpawnObject spawnedObject, float speed){
		spawnedObject.GetComponent<SpriteRenderer> ().color = Globals.getRandomColor ();
		spawnedObject.moveTo (spawnTarget.getRandomPointInVolume (),speed);
	}
}
