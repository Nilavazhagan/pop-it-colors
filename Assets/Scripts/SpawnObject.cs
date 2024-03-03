using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
	private Vector3 targetPosition;
	private bool move = false;
	float speed = 1.0f;
	public GameObject destructionParticles;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (move) {
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);
		}
		if (Vector3.Distance (transform.position, targetPosition) < 0.001f) {
			move = false;
			if (Globals.instance.currentGameMode != Globals.GameModes.TIMED && !(Globals.instance.currentGameMode == Globals.GameModes.RANDOM && Globals.instance.maxSimultaneousSpawns == 15)) {
				Globals.UI.showGameOverScreen ();
			} else {
				GameObject.FindObjectOfType<MainScript> ().free ();
			}
			Destroy (gameObject);
		}
	}

	public void destroy(bool updateScore = true){
		Vector2 currentPosition = transform.position;
		GameObject instantiatedParticle =  Instantiate (destructionParticles, currentPosition,new Quaternion());
		ParticleSystem.MainModule mainModule = instantiatedParticle.GetComponent<ParticleSystem> ().main;
		mainModule.startColor = gameObject.GetComponent<SpriteRenderer> ().color;
		instantiatedParticle.GetComponent<ParticleSystem> ().Play ();
		Destroy (gameObject);
		if (!updateScore) {
			return;
		}
		GameObject.FindObjectOfType<MainScript> ().free ();
		ScoreTicker.instance.tick ();
		if (ScoreTicker.instance.getScore() % Globals.instance.addTimeForMultiplesOf == 0) {
			Globals.UI.addBonusTime (currentPosition,gameObject.GetComponent<SpriteRenderer> ().color);
		}
	}

	public void moveTo(Vector3 targetPos,float speed){
		targetPosition = targetPos;
		this.speed = speed;
		move = true;
	}
}
