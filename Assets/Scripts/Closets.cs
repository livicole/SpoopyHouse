using UnityEngine;
using System.Collections;

public class Closets : MonoBehaviour
{

	private Animator closetAnimator;
	Transform flashlight;

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
				}
			}

			if (rayHitInfo.collider.gameObject == this.gameObject && this.gameObject.tag == "rightDoor") {
				if (Input.GetButtonDown ("Use")) {
					closetAnimator.SetTrigger ("ClosetTrigger");
				}
			}
		}
	}
}
