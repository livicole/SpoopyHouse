using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float totalTime = 300f;

	void Update(){
<<<<<<< HEAD

=======
>>>>>>> 276e5183f64637454431d3c8518ede4d6a79df3a
		string timerTextInSeconds = string.Format ("{0}:{1}", Mathf.Floor (totalTime / 60), Mathf.Floor (totalTime % 60));
		totalTime -= Time.deltaTime;
        timerText.text = timerTextInSeconds;

		if (totalTime <= 0) {
			//child loses
		}
	}
}
