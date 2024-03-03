using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour {
	private Spawner[] spawners;
	private int busyCounter = 0;
	private float speed = 1.0f;
	//private int maxSpawns = 1;
	private int[] allowedGameModes = new int[] {(int)Globals.GameModes.EASY,(int)Globals.GameModes.MEDIUM,(int)Globals.GameModes.HARD,(int)Globals.GameModes.TIMED};
	public float spawnObjectAcceleration = 0.1f;
	private string[] sequence;
	private int currentIndex = 0;

	// Use this for initialization
	void Start () {
		spawners =  GameObject.FindObjectsOfType<Spawner> ();
		if (Globals.instance.currentGameMode == Globals.GameModes.NUMBERS) {
			sequence = new string[Globals.instance.maxSimultaneousSpawns];
			for (int i = 0; i < Globals.instance.maxSimultaneousSpawns; i++)
				sequence [i] = (i + 1).ToString();
		}
		//if (Globals.instance.currentGameMode == Globals.GameModes.RANDOM) {
		//	Globals.instance.maxSimultaneousSpawns = allowedGameModes [Random.Range (0, allowedGameModes.Length)];
		//} else if (Globals.instance.currentGameMode != Globals.GameModes.PROGRESSIVE) {
		//	Globals.instance.maxSimultaneousSpawns = (int)Globals.instance.currentGameMode;
		//}
	}
	
	// Update is called once per frame
	void Update () {
		emitDots ();
		listenToTouchEvent ();
		if (Globals.instance.currentGameMode == Globals.GameModes.TIMED && ScoreTicker.instance.getAvailableTime () <= 0) {
			busyCounter++;
		}
	}

	public void free(){
		busyCounter--;
	}

	private bool isBusy(){
		return busyCounter != 0;
	}

	private void emitDots(){
		//Debug.Log (isBusy ());
		if (!isBusy()) {
			if (Globals.instance.currentGameMode == Globals.GameModes.PROGRESSIVE) {
				Globals.instance.maxSimultaneousSpawns++;
			} else if (Globals.instance.currentGameMode == Globals.GameModes.RANDOM) {
				Globals.instance.maxSimultaneousSpawns = allowedGameModes [Random.Range (0, allowedGameModes.Length)];
			} else if (Globals.instance.currentGameMode == Globals.GameModes.NUMBERS) {
				shuffleSequence ();
				ScoreTicker.instance.updateSequencer (sequence);
				currentIndex = 0;
			}
			for (;busyCounter < Globals.instance.maxSimultaneousSpawns; busyCounter++) {
				int randomIndex = UnityEngine.Random.Range (0, spawners.Length);
				spawners [randomIndex].spawnADot (speed,busyCounter + 1);
			}
			speed += spawnObjectAcceleration;
		}
	}

	private void shuffleSequence(){
		var length = sequence.Length;
		for (int i = 0; i < length - 1; i++) {
			//Generating random index to swap with i
			int randomIndex = UnityEngine.Random.Range (i, length);
			string temp = sequence [i];
			sequence [i] = sequence [randomIndex];
			sequence [randomIndex] = temp;
		}
	}

	private bool isValidHit(NumberedSpawnObject target){
		string targetNumber = target.getNumber ().ToString();
		bool isValid = targetNumber.Equals (sequence [currentIndex]);
		currentIndex++;
		return isValid;
	}

	private void listenToTouchEvent(){
		#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0)){
			RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
			if(hitInfo){
				if(Globals.instance.currentGameMode == Globals.GameModes.NUMBERS){
					NumberedSpawnObject spawnObject =  hitInfo.transform.gameObject.GetComponent<NumberedSpawnObject>();
					bool validHit = isValidHit(spawnObject);
					if(validHit){
						spawnObject.destroy();
					}else{
						Globals.UI.showGameOverScreen();
					}
				}else{
					hitInfo.transform.gameObject.GetComponent<SpawnObject>().destroy();
				}
			}
		}
		#else
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch (i).phase == TouchPhase.Began) {
				RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (i).position), Vector2.zero);
				if(hitInfo){
					if(Globals.instance.currentGameMode == Globals.GameModes.NUMBERS){
						NumberedSpawnObject spawnObject =  hitInfo.transform.gameObject.GetComponent<NumberedSpawnObject>();
						bool validHit = isValidHit(spawnObject);
						if(validHit){
							spawnObject.destroy();
						}else{
							Globals.UI.showGameOverScreen();
						}
					}else{
						hitInfo.transform.gameObject.GetComponent<SpawnObject>().destroy();
					}
				}
			}
		}
		#endif
	}
}
