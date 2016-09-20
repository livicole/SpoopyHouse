using UnityEngine;
using System.Collections;

public class DetectionSphereController : MonoBehaviour {

    public Transform detectedObject;

	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
        detectedObject = other.gameObject.transform;
	}

    void OnTriggerExit (Collider other)
    {
        detectedObject = null;
    }
}
