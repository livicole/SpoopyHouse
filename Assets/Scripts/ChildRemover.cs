using UnityEngine;
using System.Collections;

public class ChildRemover : MonoBehaviour {

    [SerializeField]
    float heightDifference;

    [SerializeField]
    float detectDistance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Use"))
        {
            Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y - heightDifference, transform.position.z);

            Ray forwardRay = new Ray(rayPosition, transform.forward);
            RaycastHit forwardRayHit = new RaycastHit();
            Debug.DrawRay(rayPosition, transform.forward * detectDistance, Color.red);

            if (Physics.Raycast(forwardRay, out forwardRayHit, detectDistance))
            {
                Debug.Log(forwardRayHit.collider.gameObject.layer);
                if (forwardRayHit.collider.gameObject.layer == 11)
                {
                    
                    Destroy(forwardRayHit.collider.gameObject);
                }
            }
        }
    }
}
