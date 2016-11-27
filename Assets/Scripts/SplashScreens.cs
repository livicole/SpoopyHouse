using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreens : MonoBehaviour {

	bool loadScene;

	public Image loadingScreen;

	[SerializeField]
	private Text loadingText;

	void Start(){
		loadingScreen.enabled = false;
	}

	void Update(){
		if (loadScene == true) {
			loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1));
		}
	}

	public void HitTutorial(){
		SceneManager.LoadScene (1);
	}

	public void HitNoTutorial(){
		loadScene = true;
		loadingScreen.enabled = true;
		loadingText.text = "LOADING";
		StartCoroutine (LoadGameScene ());
	}

	IEnumerator LoadGameScene(){
		yield return new WaitForSeconds (2);
		AsyncOperation async = SceneManager.LoadSceneAsync (2);

		while (!async.isDone) {
			yield return null;
		}
	}
}
