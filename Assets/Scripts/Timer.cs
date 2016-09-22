using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float secondsCount;
	private float minuteCount;

	void Update(){
		UpdateTimerUI();
	}

	public void UpdateTimerUI(){
		//set timer UI

		string minutes = minuteCount.ToString("00");
		string seconds = Mathf.Floor(secondsCount % 60).ToString("00");
		secondsCount += Time.deltaTime;
		timerText.text = minutes + ":" + seconds;
		if (secondsCount >= 60) {
			minuteCount++;
			secondsCount = 0;   
		}
	}
}
