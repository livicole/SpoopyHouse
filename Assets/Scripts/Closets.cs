using UnityEngine;
using System.Collections;

public class Closets : MonoBehaviour
{

	private Animator closetAnimator;
	Transform flashlight;
	public AudioSource soundManager;
	public AudioClip closetOpen;
	public AudioClip closetClose;

	// Use this for initialization
	void Start ()
	{
		closetAnimator = GetComponent<Animator> ();
		flashlight = GameObject.Find ("Flashlight").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		flashlight = GameObject.Find ("Flashlight").transform;
		Ray closetRay = new Ray (flashlight.position, flashlight.forward);
		RaycastHit rayHitInfo = new RaycastHit ();
		if (Physics.Raycast (closetRay, out rayHitInfo, 1000f)) {
			if (rayHitInfo.collider.gameObject == this.gameObject && this.gameObject.tag == "leftDoor") {
				if (Input.GetButtonDown ("Use")) {
					closetAnimator.SetTrigger ("ClosetTrigger");
//					if (closetAnimator.GetCurrentAnimatorStateInfo (0).IsName ("RClosetClosedIdle")) {
//						soundManager.PlayOneShot (closetOpen, 1.0f);
//					}
//					if (closetAnimator.GetCurrentAnimatorStateInfo (0).IsName ("RClosetOpenIdle")) {
//						soundManager.PlayOneShot (closetClose, 1.0f);
//					}
				}
			}

			if (rayHitInfo.collider.gameObject == this.gameObject && this.gameObject.tag == "rightDoor") {
				if (Input.GetButtonDown ("Use")) {
					closetAnimator.SetTrigger ("ClosetTrigger");
//					if (closetAnimator.GetCurrentAnimatorStateInfo (0).IsName ("RClosetClosedIdle")) {
//						soundManager.PlayOneShot (closetOpen, 1.0f);
//					}
//					if (closetAnimator.GetCurrentAnimatorStateInfo (0).IsName ("RClosetOpenIdle")) {
//						soundManager.PlayOneShot (closetClose, 1.0f);
//					}
				}
			}
		}
	}

	//plays closet opening sound when ClosetOpenAnim begins
	void PlayOpenSound(){
		soundManager.PlayOneShot (closetOpen, 1.0f);
	}

	//plays closet closing sound when ClosetCloseAnim begins
	void PlayCloseSound(){
		soundManager.PlayOneShot (closetClose, 1.0f);
	}
}
