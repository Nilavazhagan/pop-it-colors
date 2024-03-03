using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpContentHandler : MonoBehaviour {
	public GameObject helpPanel;
	public Text helpText;
	public Text centeredHelpText;
	public Text tapAnywhereText;
	public GameObject rectangularMask;
	public GameObject linePrefab;
	public GameObject centeredLinePrefab;
	public GameObject circularMaskPrefab;

	public GameObject debugObject;

	Globals.GameModes currentGameMode;
	bool isHelpActive = false;
	GameObject lineToDraw;
	string defaultHelpContent = "Pop the dots before they go out of the screen";
	Dictionary<Globals.GameModes,string> HelpTextMap = new Dictionary<Globals.GameModes, string>(){
		{Globals.GameModes.EASY,"Pop the dot before it goes out of the screen"},
		{Globals.GameModes.MEDIUM,"Pop the dots before they go out of the screen"},
		{Globals.GameModes.HARD,"Pop the dots before they go out of the screen"},
		{Globals.GameModes.TIMED,"Pop as many dots before the timer runs out"},
		{Globals.GameModes.NUMBERS,"Pop the dots in the correct sequence"}//,
		//{Globals.GameModes.RANDOM,"Pop the dots before it goes out of the screen"},
		//{Globals.GameModes.PROGRESSIVE,"Pop the dots before it goes out of the screen"}
	};
	// Use this for initialization
	void Start () {
		currentGameMode = Globals.instance.currentGameMode;
		lineToDraw = linePrefab;
		if (!isHelpShown ()) {
			StartCoroutine (delayAndShowHelp ());
		} else {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isHelpActive) {
			#if UNITY_EDITOR
			if(Input.GetMouseButtonDown(0)){
				Time.timeScale = 1;
				Destroy(gameObject);
			}
			#elif UNITY_ANDROID
			for (int i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					Time.timeScale = 1;
					Destroy(gameObject);
				}
			}
			#endif
		}
	}

	IEnumerator delayAndShowHelp(){
		yield return new WaitForSeconds (2);
		if (!(ScoreTicker.instance.getScore () > 0)) {
			showHelpContent ();
		} else {
			Destroy (gameObject);
		}
	}

	bool isHelpShown(){
		return Personalization.instance.getHighScore (currentGameMode) != 0;
	}

	void showHelpContent(){
		Text textToShow = helpText;
		//Pause Application
		Time.timeScale = 0;

		/*GameMode Specific Help Operations*/
		switch (currentGameMode) {
		case Globals.GameModes.EASY:
			textToShow = centeredHelpText;
			lineToDraw = centeredLinePrefab;
			//spawnMasksAndDrawLines ();
			break;
		case Globals.GameModes.MEDIUM:
			textToShow = centeredHelpText;
			lineToDraw = centeredLinePrefab;
			//spawnMasksAndDrawLines ();
			break;
		case Globals.GameModes.HARD:
			textToShow = centeredHelpText;
			lineToDraw = centeredLinePrefab;
			//spawnMasksAndDrawLines ();
			break;
		case Globals.GameModes.TIMED:
			textToShow = helpText;
			DrawLine (rectangularMask.GetComponent<RectTransform> (), textToShow.GetComponent<RectTransform> ());
			rectangularMask.SetActive (true);
			break;
		case Globals.GameModes.NUMBERS:
			textToShow = helpText;
			DrawLine (rectangularMask.GetComponent<RectTransform> (), textToShow.GetComponent<RectTransform> ());
			rectangularMask.SetActive (true);
			break;
		case Globals.GameModes.RANDOM:
			textToShow = centeredHelpText;
			break;
		case Globals.GameModes.PROGRESSIVE:
			textToShow = centeredHelpText;
			//spawnMasksAndDrawLines ();
			break;
		};

		/*Construction of Help Content*/
		string helpTextContent;
		if (!HelpTextMap.TryGetValue (currentGameMode, out helpTextContent)) {
			helpTextContent = defaultHelpContent;
		}
		textToShow.text = helpTextContent;

		/*Setting Help Content elements as active*/
		helpPanel.SetActive (true);
		textToShow.gameObject.SetActive (true);
		tapAnywhereText.gameObject.SetActive (true);
		isHelpActive = true;
	}

	void spawnMasksAndDrawLines(){
		SpawnObject[] spawnedObjects = GameObject.FindObjectsOfType<SpawnObject> ();
		for (int i = 0; i < spawnedObjects.Length; i++) {
			Vector2 maskPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (gameObject.GetComponent<RectTransform> (), Camera.main.WorldToScreenPoint (spawnedObjects [i].transform.position), Camera.main, out maskPosition);
			GameObject circularMask = Instantiate (circularMaskPrefab, Vector2.zero, Quaternion.identity);
			circularMask.transform.SetParent (gameObject.transform, false);
			circularMask.GetComponent<RectTransform> ().localPosition = maskPosition;
			//DrawLine (circularMask.GetComponent<RectTransform>(), centeredHelpText.GetComponent<RectTransform>());
		}
	}

	void DrawLine(RectTransform fromRect, RectTransform toRect){
		Vector2 from = fromRect.anchoredPosition;
		Vector2 to = toRect.anchoredPosition;
		Vector2 midpoint = new Vector2 ((to.x + from.x) / 2, (to.y + from.y) / 2);

		/*GameObject fromPoint = Instantiate (debugObject, from, new Quaternion ());
		fromPoint.transform.SetParent (gameObject.transform, false);
		fromPoint.name = "From";
		GameObject toPoint= Instantiate (debugObject, to, new Quaternion ());
		toPoint.transform.SetParent (gameObject.transform, false);
		toPoint.name = "To";
		GameObject midPoint = Instantiate (debugObject, midpoint, new Quaternion ());
		midPoint.transform.SetParent (gameObject.transform, false);
		midPoint.name = "Midpoint";*/

		//Debug.Log ("From : " + from.ToString ());
		//Debug.Log ("To : " + to.ToString ());
		//Debug.Log ("Midpoint : " + midpoint.ToString ());

		float angle = Vector2.Angle (from, to);
		Vector3 eulerRotation = new Vector3 (0, 0, angle);
		//Debug.Log ("Angle : " + angle.ToString ());
		GameObject drawnLine = Instantiate (lineToDraw, midpoint, Quaternion.Euler (eulerRotation));
		drawnLine.transform.SetParent (gameObject.transform, false);
		RectTransform lineRect = drawnLine.GetComponent<RectTransform> ();
		float height = Vector2.Distance (from, to) * 0.5f;
		//Debug.Log ("Height : " + height.ToString ());
		lineRect.sizeDelta = new Vector2 (lineRect.sizeDelta.x, height);
	}
}
