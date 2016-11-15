using UnityEngine;
using System.Collections;

public class Dresser : MonoBehaviour
{

	private Animator dresserAnimator;
	Transform flashlight; 
	public AudioSource soundManager;
	public AudioClip drawerOpen, drawerClose;

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
//					if (dresserAnimator.GetCurrentAnimatorStateInfo (0).IsName ("CloseDrawerIdle")) {
//						soundManager.PlayOneShot (drawerOpen, 1.0f);
//					}
//					if (dresserAnimator.GetCurrentAnimatorStateInfo (0).IsName ("OpenDrawerIdle")) {
//						soundManager.PlayOneShot (drawerClose, 1.0f);
//					}
				}
			}
		}
	}

	//plays drawer opening sound when DrawerOpenAnim begins
	void PlayOpenSound(){
		soundManager.PlayOneShot (drawerOpen, 1.0f);
	}

	//plays drawer closing sound when DrawerCloseAnim begins
	void PlayCloseSound(){
		soundManager.PlayOneShot (drawerClose, 1.0f);
	}
}