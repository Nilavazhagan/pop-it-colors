using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personalization : MonoBehaviour {
	static Personalization personalizationInstance;
	Dictionary<Globals.GameModes,int> highScores = new Dictionary<Globals.GameModes, int>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int getHighScore(Globals.GameModes gameMode){
		int score = 0;
		if (highScores.TryGetValue (gameMode, out score)) {
		//if(highScores.ContainsKey(gameMode)){
			//score = highScores [gameMode];
			return score;
		} else {
			score = PlayerPrefs.GetInt (gameMode.ToString (),0);
			highScores.Add (gameMode, score);
			return score;
		}
	}

	public bool setHighScore(Globals.GameModes gameMode, int score){
		int currentHighScore = getHighScore (gameMode);
		if (score > currentHighScore) {
			highScores.Remove (gameMode);
			highScores.Add (gameMode, score);
			PlayerPrefs.SetInt (gameMode.ToString(), score);
			return true;
		}
		return false;
	}

	public static Personalization instance {
		get { 
			if (personalizationInstance == null) {
				personalizationInstance = FindObjectOfType (typeof(Personalization)) as Personalization;
			}
			if (personalizationInstance == null) {
				GameObject obj = new GameObject ("GlobalGameObject");
				personalizationInstance = obj.AddComponent<Personalization> ();
			}
			return personalizationInstance;
		}
	}

	void onApplicationQuit(){
		personalizationInstance = null;
	}
}
