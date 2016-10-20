using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Breathing : MonoBehaviour {

	public AudioSource heavyBreathing;
	bool hasBeenPlaying = false;


	// Update is called once per frame
	void Update () {
		GameObject[] ghostToys = GameObject.FindGameObjectsWithTag ("Toys");
		if (ghostToys.Length == 0) {
			heavyBreathing.Stop ();
			hasBeenPlaying = false;
		}
		foreach (GameObject target in ghostToys) {

			float distance = Vector3.Distance (target.transform.position, transform.position);
			//Debug.Log (distance + " to " + target.name);
			if (distance <= 20) {
				if (!hasBeenPlaying) {
					hasBeenPlaying = true;
					heavyBreathing.Play ();
				}
			} else {
				hasBeenPlaying = false;
				heavyBreathing.Stop ();
			}
		}
	}
}
