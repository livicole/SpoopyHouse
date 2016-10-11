using UnityEngine;
using System.Collections;

public class Dresser : MonoBehaviour
{

	private Animator dresserAnimator;
	public Transform flaslight; 
	public static bool drawerOpen;

	// Use this for initialization
	void Start ()
	{
		drawerOpen = false;
		dresserAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray drawerRay = new Ray (flaslight.position, flaslight.forward);
		RaycastHit rayHitInfo = new RaycastHit ();
		if (Physics.Raycast (drawerRay, out rayHitInfo, 1000f)) {
			//Debug.DrawRay (flaslight.position, flaslight.forward * 1000f, Color.blue);
			//Debug.Log (rayHitInfo.collider.name);
			if (rayHitInfo.collider.gameObject  == this.gameObject) {
				Debug.Log ("it hit");
				if (Input.GetButton ("Ghost Button A")) {
					if (drawerOpen == false) {
						dresserAnimator.SetBool ("DrawerIsOpen", true);
						dresserAnimator.SetBool ("DrawerIsClosed", false);
						drawerOpen = true;
						Debug.Log (drawerOpen);
					} else if (drawerOpen == true) {
						dresserAnimator.SetBool ("DrawerIsOpen", false);
						dresserAnimator.SetBool ("DrawerIsClosed", true);
						drawerOpen = false;
						Debug.Log (drawerOpen);
					}
				}
			}
		}
	}
}