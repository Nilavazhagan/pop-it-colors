using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTicker : MonoBehaviour {
	static ScoreTicker tickerInstance = null;
	int score = 0;
	Text scoreText;
	public GameObject timerObject;
	public GameObject sequencerObject;
	private int timer = 61;

	void Awake(){
		tickerInstance = this;
		scoreText = gameObject.GetComponent<Text> ();
		scoreText.text = score.ToString();
	}

	void Start(){
		if (Globals.instance.currentGameMode == Globals.GameModes.TIMED) {
			timerObject.SetActive (true);
			InvokeRepeating ("CountDown", 0f, 1.0f);
		}else if (Globals.instance.currentGameMode == Globals.GameModes.NUMBERS) {
			sequencerObject.SetActive (true);
		}
	}

	void CountDown(){
		if (timer != 0) {
			timer--;
			Text timerText = timerObject.GetComponent<Text> ();
			timerText.text = timer.ToString () + "s";
		}
		if (timer == 0) {
			Globals.UI.showGameOverScreen ();
		}
	}
	public int getAvailableTime(){
		return timer;
	}
	public void tick(){
		score++;
		scoreText.text = score.ToString();
		if (score % Globals.instance.addTimeForMultiplesOf == 0) {
			timer += Globals.instance.bonusTime;
		}
	}
	public int getScore(){
		return score;
	}

	public void updateSequencer(string[] sequence){
		Text sequencerText = sequencerObject.GetComponent<Text> ();
		sequencerText.text = string.Join (" ", sequence);
	}
	public static ScoreTicker instance {
		get { 
			if (tickerInstance == null) {
				tickerInstance = FindObjectOfType (typeof(ScoreTicker)) as ScoreTicker;
			}
			if (tickerInstance == null) {
				GameObject obj = new GameObject ("ScoreTicker");
				tickerInstance = obj.AddComponent<ScoreTicker> ();
			}
			return tickerInstance;
		}
	}

	void onApplicationQuit(){
		tickerInstance = null;
	}
}
