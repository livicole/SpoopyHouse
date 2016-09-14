using UnityEngine;
using System.Collections;

public class GhostCameraController : MonoBehaviour {

    [SerializeField]
    float panSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float inputX = Input.GetAxis("HorizontalCamera2");
        float inputY = Input.GetAxis("VerticalCamera2");

        Vector3 movementVector = new Vector3(inputX, 0, inputY);
        movementVector = movementVector.normalized * panSpeed;
	
	}
}
