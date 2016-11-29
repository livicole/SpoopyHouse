using UnityEngine;
using System.Collections;

public class ClockHand : MonoBehaviour
{

	public float rotateAmount = 1.5f;

	void Awake ()
	{
		StartCoroutine (TurnHand ());
	}

	IEnumerator TurnHand ()
	{
		while (true) {
			Timer timerScript = GameObject.Find ("GameManager").GetComponent<Timer> ();
			float rotateSpeed = timerScript.timeBetweenClock;

			this.gameObject.transform.Rotate (0f, 0f, rotateAmount);
			yield return new WaitForSeconds (rotateSpeed);
		}
	}
}
