using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	public float totalTime;

	void Update(){
		string timerTextInSeconds = string.Format ("{1:00}", Mathf.Floor (totalTime / 60), totalTime % 60);
		totalTime -= Time.deltaTime;
		timerText.text = timerTextInSeconds.ToString ();

		if (totalTime <= 0) {
			//child loses
		}
	}
}
