using UnityEngine;
using System.Collections;

public class DetectionSphereController : MonoBehaviour {

    [SerializeField]
    float speed = 1f;

    [SerializeField]
    Transform cursor;

    public Transform detectedObject;

    private Vector3 targetLocation;
    public bool moving = false, invisGrid = false;


    void Update()
    {
        Physics.IgnoreLayerCollision(5, 12, true);
        Physics.IgnoreLayerCollision(5, 13, true);
        if (!invisGrid)
        {
            Physics.IgnoreLayerCollision(5, 15, true);
        }
        Physics.IgnoreLayerCollision(5, 10, true);
        Physics.IgnoreLayerCollision(5, 5, true);

       // Physics.IgnoreCollision(GetComponent<Collider>(), cursor.GetComponent<CursorController>().holdingObject.GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("ChildPlayer").GetComponent<Collider>(), true);

        if (moving)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<DetectionSphereController>().moving = true;
            GetComponent<Collider>().isTrigger = false;

            targetLocation = cursor.GetComponent<CursorController>().targetLocation;
            //targetLocation.y = -0.8f;
           
            Vector3 targetVector = (targetLocation - transform.position);
            Debug.Log("Target vector : " + targetVector);
            //targetVector = targetVector;

            GetComponent<Rigidbody>().velocity = targetVector.normalized * 100;
        }
        else
        {
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<DetectionSphereController>().moving = false;
            transform.position = cursor.GetComponent<CursorController>().targetLocation;
        }


        //GetComponent<CharacterController>().Move(new Vector3(inputX, 0, -inputY) * speed);
    }

	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
        detectedObject = other.gameObject.transform;
	}

    void OnTriggerExit(Collider other)
    {
        detectedObject = null;
    }
}
