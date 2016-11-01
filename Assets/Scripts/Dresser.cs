using UnityEngine;
using System.Collections;

public class Dresser : MonoBehaviour
{

	private Animator dresserAnimator;
	Transform flashlight; 
	// Use this for initialization
	void Start ()
	{
		dresserAnimator = GetComponent<Animator> ();
        flashlight = GameObject.Find("Flashlight").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
        flashlight = GameObject.Find("Flashlight").transform;
        Ray drawerRay = new Ray (flashlight.position, flashlight.forward);
		RaycastHit rayHitInfo = new RaycastHit ();
		if (Physics.Raycast (drawerRay, out rayHitInfo, 1000f)) {
			//Debug.DrawRay (flaslight.position, flaslight.forward * 1000f, Color.blue);
			//Debug.Log (rayHitInfo.collider.name);
			if (rayHitInfo.collider.gameObject  == this.gameObject) {
			//	Debug.Log ("it hit");
				if (Input.GetButtonDown ("Use")) {
					dresserAnimator.SetTrigger ("Trigger");
				}
			}
		}
	}
}