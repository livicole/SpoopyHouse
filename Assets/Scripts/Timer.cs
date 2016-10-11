using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float totalTime = 300f;

	void Update(){
		string timerTextInSeconds = string.Format ("{0}:{1}", Mathf.Floor (totalTime / 60), Mathf.Floor (totalTime % 60));
		totalTime -= Time.deltaTime;
        timerText.text = timerTextInSeconds;

		if (totalTime <= 0) {
			//child loses
		}
	}
}
