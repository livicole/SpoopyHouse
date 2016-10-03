using UnityEngine;
using System.Collections;

public class Dresser : MonoBehaviour {

	private Animator dresserAnimator;

	// Use this for initialization
	void Start () {
		dresserAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			dresserAnimator.SetBool ("DrawerIsOpen", true);
			dresserAnimator.SetBool ("DrawerIsClosed", false);
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			dresserAnimator.SetBool ("DrawerIsClosed", true);
			dresserAnimator.SetBool ("DrawerIsOpen", false);
		}
	
	}
}
