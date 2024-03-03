using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Globals : MonoBehaviour {
	public static Globals globalInstance = null;
	public enum GameModes {
		EASY = 1,
		MEDIUM = 2,
		HARD = 3,
		TIMED = 15,
		RANDOM = 200,
		PROGRESSIVE = 201,
		NUMBERS = 202
	}
	public Color[] Colors;
	public int maxSimultaneousSpawns = 0;
	public int addTimeForMultiplesOf = 15;
	public int bonusTime = 5;
	public Color backgroundColor;
	public Color textColor;
	private GameModes _currentGameMode = GameModes.EASY;
	private bool backButtonPressed = false;

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
		if (GameObject.FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
			return;
		}
		SceneManager.sceneLoaded += sceneChanged;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(!backButtonPressed){
				showToast();
				backButtonPressed = true;
				StartCoroutine(invalidateBackButtonPress());
			}else{
				Application.Quit();
			}
		}
	}

	void showToast(){
		#if UNITY_ANDROID
		AndroidJavaClass ToastClass = new AndroidJavaClass("android.widget.Toast");
		object[] toastParams = new object[3];
		AndroidJavaClass unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		toastParams[0] = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");
		toastParams[1] = "Press again to quit";
		toastParams[2] = ToastClass.GetStatic<int>("LENGTH_SHORT");
		AndroidJavaObject toastObject = ToastClass.CallStatic<AndroidJavaObject>("makeText",toastParams);
		toastObject.Call("show");
		#endif
	}

	IEnumerator invalidateBackButtonPress(){
		yield return new WaitForSeconds (3f);
		backButtonPressed = false;
	}

	void sceneChanged (Scene scene, LoadSceneMode mode){
		Camera.main.backgroundColor = backgroundColor;
		GameObject[] textObjects = GameObject.FindGameObjectsWithTag ("MainText");
		foreach (GameObject tO in textObjects) {
			tO.GetComponent<Text> ().color = textColor;
		}
	}

	public static Color getRandomColor(){
		Color[] colors = Globals.instance.Colors; 
		int randomIndex = UnityEngine.Random.Range (0, colors.Length);
		return colors [randomIndex];
	}

	public GameModes currentGameMode {
		get { 
			return _currentGameMode;
		}
		set{ 
			switch (value) {
			case GameModes.EASY:
			case GameModes.MEDIUM:
			case GameModes.HARD:
			case GameModes.TIMED:
				maxSimultaneousSpawns = (int)value;
				break;
			case GameModes.NUMBERS:
				maxSimultaneousSpawns = 4;
				break;
			case GameModes.RANDOM:
			case GameModes.PROGRESSIVE:
			default :
				maxSimultaneousSpawns = 0;
				break;
			}
			_currentGameMode = value;
		}
	}

	public static Globals instance {
		get { 
			if (globalInstance == null) {
				globalInstance = FindObjectOfType (typeof(Globals)) as Globals;
			}
			if (globalInstance == null) {
				GameObject obj = new GameObject ("GlobalGameObject");
				globalInstance = obj.AddComponent<Globals> ();
			}
			return globalInstance;
		}
	}

	public static UserInteraction UI{
		get { 
			return GameObject.FindObjectOfType<UserInteraction> ();
		}
	}

	void onApplicationQuit(){
		globalInstance = null;
	}
}
