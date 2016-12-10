using UnityEngine;
using System.Collections;

public class Closets : MonoBehaviour
{

	private Animator closetAnimator;
	Transform flashlight;
	AudioSource soundManager;
	public AudioClip closetOpen;
	public AudioClip closetClose;
	Transform leftCursor, rightCursor;

	// Use this for initialization
	void Start ()
	{
		soundManager = GameObject.Find ("SoundManager").GetComponent<AudioSource> ();
		if (this.gameObject.tag == "leftDoor") {
			leftCursor = this.gameObject.GetComponent<Transform> ().Find ("LeftQuad");
			leftCursor.GetComponent<MeshRenderer> ().enabled = false;
		}
		if (this.gameObject.tag == "rightDoor") {
			rightCursor = this.gameObject.GetComponent<Transform> ().Find ("RightQuad");
			rightCursor.GetComponent<MeshRenderer> ().enabled = false;
		}
		closetAnimator = GetComponent<Animator> ();
		flashlight = GameObject.Find ("Flashlight").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		flashlight = GameObject.Find ("Flashlight").transform;
		Ray closetRay = new Ray (flashlight.position, flashlight.forward);
		RaycastHit rayHitInfo = new RaycastHit ();
		if (Physics.Raycast (closetRay, out rayHitInfo, 5f)) {
			if (rayHitInfo.collider.gameObject == this.gameObject && this.gameObject.tag == "leftDoor") {
				leftCursor.GetComponent<MeshRenderer> ().enabled = true;
				leftCursor.transform.LookAt (flashlight);
				if (Input.GetButtonDown ("Use")) {
					closetAnimator.SetTrigger ("ClosetTrigger");
				}
			} else {
				if (this.gameObject.tag == "leftDoor"){
				leftCursor.GetComponent<MeshRenderer> ().enabled = false;
				}
			}

			if (rayHitInfo.collider.gameObject == this.gameObject && this.gameObject.tag == "rightDoor") {
				rightCursor.GetComponent<MeshRenderer> ().enabled = true;
				rightCursor.transform.LookAt (flashlight);
				if (Input.GetButtonDown ("Use")) {
					closetAnimator.SetTrigger ("ClosetTrigger");
				}
			} else {
				if(this.gameObject.tag == "rightDoor"){
				rightCursor.GetComponent<MeshRenderer> ().enabled = false;
				}
			}
		}
	}

	//plays closet opening sound when ClosetOpenAnim begins
	void PlayOpenSound ()
	{
		soundManager.PlayOneShot (closetOpen, 1.0f);
	}

	//plays closet closing sound when ClosetCloseAnim begins
	void PlayCloseSound ()
	{
		soundManager.PlayOneShot (closetClose, 1.0f);
	}
}
