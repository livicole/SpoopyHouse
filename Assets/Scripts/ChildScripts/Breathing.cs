using UnityEngine;
using System.Collections;

public class Breathing : MonoBehaviour {

	public AudioSource heavyBreathing;

	// Update is called once per frame
	void Update () {
		GameObject[] ghostToys = GameObject.FindGameObjectsWithTag ("Toys");
		foreach (GameObject target in ghostToys) {
			float distance = Vector3.Distance (target.transform.position, transform.position);
			if (distance <= 20) {
				heavyBreathing.enabled = true;
				heavyBreathing.loop = true;
			} else if (distance > 20) {
				heavyBreathing.enabled = false;
				heavyBreathing.loop = false;
			}
		}
	}
}
