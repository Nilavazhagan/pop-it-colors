using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEgg : MonoBehaviour {
	/*private Vector2 spawnPosition;
	private Vector2 destination;
	public GameObject dotPrefab;
	private GameObject spawnedObject;

	void Start(){
		spawnedObject = GameObject.FindObjectOfType<SpawnObject> ().gameObject;
		spawnedObject.GetComponent<SpawnObject> ().isEasterEggDot = true;
		destination = spawnedObject.transform.position;
		spawnPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height));
		spawnPosition.x += 0.5f;
		spawnPosition.y -= 0.5f;
	}

	void Update(){
		listenToTouchEvent ();
	}

	private void spawnAnotherDot(){
		spawnedObject = Instantiate (dotPrefab, spawnPosition, new Quaternion());
		spawnedObject.GetComponent<SpawnObject> ().isEasterEggDot = true;
		spawnedObject.GetComponent<SpriteRenderer> ().color = Color.white;
		spawnedObject.GetComponent<SpawnObject> ().moveTo (destination,1.0f);
	}

	private void listenToTouchEvent(){
		#if (UNITY_EDITOR || UNITY_STANDALONE)
		if(Input.GetMouseButtonDown(0)){
			RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
			if(hitInfo){
				hitInfo.transform.gameObject.GetComponent<SpawnObject>().destroy();
				//spawnAnotherDot();
			}
		}
		#else
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch (i).phase == TouchPhase.Began) {
				RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (i).position), Vector2.zero);
				if (hitInfo) {
					hitInfo.transform.gameObject.GetComponent<SpawnObject>().destroy();
					//spawnAnotherDot();
				}
			}
		}
		#endif
	}*/
	public GameObject destructionParticles;

	void OnMouseDown(){
		GameObject instantiatedParticle =  Instantiate (destructionParticles, transform.position,new Quaternion());
		ParticleSystem.MainModule mainModule = instantiatedParticle.GetComponent<ParticleSystem> ().main;
		mainModule.startColor = Color.white;
		instantiatedParticle.GetComponent<ParticleSystem> ().Play ();
		Destroy (gameObject);
	}
}
