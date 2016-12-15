using UnityEngine;
using System.Collections;

public class KeyItemPickUp : MonoBehaviour
{

	Transform flashlight;
	public AudioSource soundManager;
	public AudioClip chimeSound;

	void Start ()
	{
		flashlight = GameObject.Find ("Flashlight").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray keyItemRay = new Ray (this.transform.position, this.transform.forward);
		RaycastHit rayHitInfo = new RaycastHit ();
		if (Physics.Raycast (keyItemRay, out rayHitInfo, 10f)) {
			if (rayHitInfo.collider.gameObject == flashlight.gameObject) {
				if (Input.GetButtonDown ("Use")) {
					soundManager.PlayOneShot (chimeSound, 1f);
					//inventoryObject.GetComponent<InventoryScript>().pickUp(forwardRayHit.collider.gameObject);
					NewInventoryScript invScript = GameObject.Find("flashlightPlayer").GetComponent<NewInventoryScript> ();
					if (this.gameObject.name == "NauthizItem") {
						invScript.nauthCollected = true;
					}
					if (this.gameObject.name == "OthalaItem") {
						invScript.othalCollected = true;
					}
					if (this.gameObject.name == "PerthroItem") {
						invScript.perthCollected = true;
					}
					if (this.gameObject.name == "ThurisazItem") {
						invScript.thuriCollected = true;
					}
					if (this.gameObject.name == "AlgizItem") {
						invScript.algizCollected = true;

					}

					invScript.itemsCollected++;
					this.gameObject.SetActive (false);
				}
			}
		}
	}
}
