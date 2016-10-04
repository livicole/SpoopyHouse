using UnityEngine;
using System.Collections;

public class GhostCameraController : MonoBehaviour {

    [SerializeField]
    float panSpeed;

    [SerializeField]
    float zoomSpeed;

    [SerializeField]
    float zoomMin, zoomMax, xMin, xMax, zMin, zMax;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float inputX = Input.GetAxis("HorizontalCamera2");
        float inputY = -Input.GetAxis("VerticalCamera2");

        float left = -Input.GetAxis("LeftTrigger"); //-1 -> 0
        float right = Input.GetAxis("RightTrigger"); //0 -> 1
        float combination = left + right;
        //inputX = Input.GetAxis("LeftTrigger");
        float inputZ = combination;
        //Debug.Log(inputZ);

        Vector3 movementVector = new Vector3(inputX, 0, inputY);
        movementVector = movementVector.normalized * panSpeed;

        Vector3 zoomVector = new Vector3(0, 
            inputZ * zoomSpeed
            , 0);

        Vector3 totalMovement = movementVector + zoomVector;

        Vector3 newPosition = transform.position + totalMovement * Time.deltaTime;
        newPosition = new Vector3(Mathf.Clamp(newPosition.x, xMin, xMax),
            Mathf.Clamp(newPosition.y, zoomMin, zoomMax),
            Mathf.Clamp(newPosition.z, zMin, zMax));
        transform.position = newPosition;


    }
}
