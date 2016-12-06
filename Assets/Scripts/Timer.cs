using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

	public Text timerText;
	private Text winText;
	public float totalTime = 600f;

	public float timeBetweenClock;
	float clockVolume;

	public AudioSource soundManager;
	public AudioClip tickSound;
	public AudioClip tockSound;


	void Awake ()
	{
		StartCoroutine (ClockTicking ());

		if (totalTime > 10) {
			StartCoroutine (DecreaseTickSound ());
		}
	}

	void Start ()
	{
		timeBetweenClock = 1f;
		clockVolume = 0.2f;
	}

	void Update ()
	{
		if (totalTime > 0) {
			string timerTextInSeconds = string.Format ("{0}:{1:00}", Mathf.Floor (totalTime / 60), Mathf.Floor (totalTime % 60));
			totalTime -= Time.deltaTime;
			timerText.text = timerTextInSeconds;
		} else {
			timerText.text = "0:00";
			winText = GameObject.Find ("WinText").GetComponent<Text> ();
			winText.text = "Boo Win";
			GetComponent<GameManager> ().gameIsLive = false;
		}

		if (totalTime < 10) {
			timeBetweenClock = 0.2f;
		}

		if (totalTime <= 0) {
			StopCoroutine (ClockTicking ());
		}
	}

	IEnumerator DecreaseTickSound ()
	{
		while (true) {
			yield return new WaitForSeconds (180);
			timeBetweenClock -= 0.2f;
			//clockVolume += 0.2f;
			Debug.Log (timeBetweenClock + "time");
			Debug.Log (clockVolume + "volume");
		}
	}

	IEnumerator ClockTicking ()
	{
		while (true) {
			//yield return new WaitForSeconds (1);
			soundManager.PlayOneShot (tickSound, clockVolume);
			yield return new WaitForSeconds (timeBetweenClock);
			soundManager.PlayOneShot (tockSound, clockVolume);
			yield return new WaitForSeconds (timeBetweenClock);
		}
	}
}
