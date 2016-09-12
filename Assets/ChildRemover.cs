using UnityEngine;
using System.Collections;

public class ChildRemover : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Use"))
        {
            Ray forwardRay = new Ray(transform.position, Vector3.forward);
            RaycastHit forwardRayHit = new RaycastHit();

            if (Physics.Raycast(forwardRay, out forwardRayHit, 100f))
            {
                if (forwardRayHit.collider.gameObject.layer.Equals("Haunted"))
                {
                    Destroy(forwardRayHit.collider.gameObject);
                }
            }
        }
    }
}
