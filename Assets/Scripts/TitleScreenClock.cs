using UnityEngine;
using System.Collections;

public class TitleScreenClock : MonoBehaviour {

	public float rotateAmount = 1.5f;
	public AudioSource soundManager;
	public AudioClip tickSound;
	public AudioClip tockSound;

	void Awake(){
		StartCoroutine (TurnHand ());
		StartCoroutine (ClockTicking ());
	}

	IEnumerator TurnHand ()
	{
		while (true) {
			this.gameObject.transform.Rotate (0f, 0f, rotateAmount);
			yield return new WaitForSeconds (1);
		}
	}

	IEnumerator ClockTicking ()
	{
		while (true) {
			//yield return new WaitForSeconds (1);
			soundManager.PlayOneShot (tickSound, 0.4f);
			yield return new WaitForSeconds (1);
			soundManager.PlayOneShot (tockSound, 0.4f);
			yield return new WaitForSeconds (1);
		}
	}
}
