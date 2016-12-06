using UnityEngine;
using System.Collections;

public class KeyItemPickUp : MonoBehaviour
{

	Transform flashlight;

	void Start ()
	{
		flashlight = GameObject.Find ("Flashlight").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray keyItemRay = new Ray (flashlight.position, flashlight.forward);
		RaycastHit rayHitInfo = new RaycastHit ();
		if (Physics.Raycast (keyItemRay, out rayHitInfo, 5f)) {
			if (rayHitInfo.collider.gameObject == this.gameObject) {
				if (Input.GetButtonDown ("Use")) {
					//inventoryObject.GetComponent<InventoryScript>().pickUp(forwardRayHit.collider.gameObject);
					NewInventoryScript invScript = GameObject.Find("ChildPlayer").GetComponent<NewInventoryScript> ();
					invScript.itemsCollected++;
					this.gameObject.SetActive (false);
				}
			}
		}
	}
}
