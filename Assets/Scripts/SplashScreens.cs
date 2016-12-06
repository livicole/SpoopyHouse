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

	[SerializeField]
	private Text loadingText;

	void Start(){
		loadingScreen.enabled = false;
		loadingText.enabled = false;
		playButton.enabled = true;
		controlButton.enabled = true;
		creditButton.enabled = true;
	}

	void Update(){
		if (loadScene == true) {
			loadingText.enabled = true;
			playButton.enabled = false;
			controlButton.enabled = false;
			creditButton.enabled = false;
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
