using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreens : MonoBehaviour {

	bool loadScene;

	public Image loadingScreen;
	public Button playButton;
	public Button controlButton;
	public Button creditButton;
	string[] tipText = new string[]{"As the child, press A near a Haunted Object to destroy it.", "As the ghost, press L3 to zoom to the child's location.", "Rooms cannot be moved if they're not connected to at least one other room.",
		"As the child, press X to drop a lamp, causing the ghost to be unable to move that room until you pick it back up.", "As the ghost, use the bumpers to rotate a selected room.", "The child's goal is to collect runes and escape the house. Listen to your surroundings to help find the runes.",
		"As the child, heavy breathing implies there's a haunted object nearby."};

	[SerializeField]
	private Text loadingText;
	public Text tipTextUI;

	void Start(){
		loadingScreen.enabled = false;
		loadingText.enabled = false;
		tipTextUI.enabled = false;
		playButton.enabled = true;
		controlButton.enabled = true;
		creditButton.enabled = true;
		int randomIndex = Random.Range(0 , tipText.Length);
		tipTextUI.text = tipText[randomIndex];
	}

	void Update(){
		if (loadScene == true) {
			loadingText.enabled = true;
			loadingScreen.enabled = true;
			playButton.enabled = false;
			controlButton.enabled = false;
			creditButton.enabled = false;
			tipTextUI.enabled = true;
			loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1));
		}
	}

	public void PressedPlay(){
		StartCoroutine (LoadGameScene ());
	}
		
	IEnumerator LoadGameScene(){
		loadScene = true;
		yield return new WaitForSeconds (2);
		AsyncOperation async = SceneManager.LoadSceneAsync (1);

		while (!async.isDone) {
			yield return null;
		}
	}
}
