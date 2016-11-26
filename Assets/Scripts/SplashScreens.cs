using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreens : MonoBehaviour {
	
	public void HitTutorial(){
		SceneManager.LoadScene (1);
	}

	public void HitNoTutorial(){
		SceneManager.LoadScene (4);
	}
}
