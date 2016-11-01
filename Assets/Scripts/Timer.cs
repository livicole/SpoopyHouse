using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
    private Text winText;
	public float totalTime = 71f;

	void Update(){

        if(totalTime > 0)
        {
            string timerTextInSeconds = string.Format("{0}:{1:00}", Mathf.Floor(totalTime / 60), Mathf.Floor(totalTime % 60));
            totalTime -= Time.deltaTime;
            timerText.text = timerTextInSeconds;
        }
        else
        {
            timerText.text = "0:00";
            winText = GameObject.Find("WinText").GetComponent<Text>();
            winText.text = "Boo Win";
            GetComponent<GameManager>().gameIsLive = false;
            
        }
	}
}
