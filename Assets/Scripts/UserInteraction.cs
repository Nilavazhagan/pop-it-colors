using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UserInteraction : MonoBehaviour {
	private const string MainMenuScene = "MainMenuScene";
	private const string MainScene = "MainScene";

	public GameObject gameOverScreen = null;
	public GameObject highScoreIndicator = null;
	public GameObject bonusTimeObj;

	public void loadGameScene(string gameMode){
		Globals.instance.currentGameMode = (Globals.GameModes)System.Enum.Parse (typeof(Globals.GameModes), gameMode);
		loadScene (MainScene);
	}

	public void loadScene(string sceneName){
		GameObject currentButton = EventSystem.current.currentSelectedGameObject;
		AudioSource audioSource = currentButton.GetComponent<AudioSource> ();
		if (audioSource != null) {
			audioSource.Play ();
			StartCoroutine(delayedLoadScene (audioSource.clip.length, sceneName));
		} else {
			StartCoroutine (asyncLoadScene (sceneName));
		}
	}

	IEnumerator delayedLoadScene(float delay, string sceneName){
		yield return new WaitForSeconds (delay);
		StartCoroutine (asyncLoadScene (sceneName));
	}

	//To be used when Unity supports multiple arguments and enums as arguments
	/*public void loadScene(string sceneName, Globals.GameModes gameMode){
		Globals.instance.currentGameMode = gameMode;
		loadScene (sceneName);
	}*/

	IEnumerator asyncLoadScene(string sceneName){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

	public void showGameOverScreen(){
		if (gameOverScreen != null) {
			gameOverScreen.SetActive (true);
			bool isNewHighscore = Personalization.instance.setHighScore (Globals.instance.currentGameMode, ScoreTicker.instance.getScore ());
			if (isNewHighscore) {
				showHighScoreIndicator ();
			}
			SpawnObject[] spawnedObjects = GameObject.FindObjectsOfType<SpawnObject> ();
			foreach (SpawnObject SO in spawnedObjects) {
				SO.destroy (false);
			}
		}
	}

	public void showHighScoreIndicator(){
		if (highScoreIndicator != null) {
			highScoreIndicator.SetActive (true);
		}
	}

	public void addBonusTime(Vector2 position,Color color){
		if (Globals.instance.currentGameMode != Globals.GameModes.TIMED) {
			return;
		}
		Canvas canvasObj = GameObject.FindObjectOfType<Canvas> ();
		Vector2 rectPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (canvasObj.GetComponent<RectTransform> (), Camera.main.WorldToScreenPoint (position), Camera.main, out rectPosition);
		GameObject bonusTime = Instantiate (bonusTimeObj, Vector2.zero, Quaternion.identity);
		bonusTime.transform.SetParent (canvasObj.transform, false);
		bonusTime.GetComponent<RectTransform> ().localPosition = rectPosition;
		bonusTime.GetComponent<Text> ().color = color;
	}
}
