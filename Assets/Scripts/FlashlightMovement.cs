using UnityEngine;
using System.Collections;

public class FlashlightMovement : MonoBehaviour {

    private Transform child;

    private float targetXRotation, targetYRotation, targetXRotationV, targetYRotationV;

    public float rotateSpeed = 0.3f;

    public float holdHeight = 0f;
    public float holdSide = 0f;


	// Use this for initialization
	void Start () {
        targetXRotationV = 1f;
        targetYRotationV = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        child = GameObject.Find("ChildPlayer").transform;
        targetXRotation = Mathf.SmoothDamp(targetXRotation, child.FindChild("ChildCamera").GetComponent<CameraController>().xRotation, ref targetXRotationV, rotateSpeed);
        targetYRotation = Mathf.SmoothDamp(targetYRotation, child.GetComponent<PlayerMovement>().yRotation, ref targetYRotationV, rotateSpeed);
        Vector3 offset = new Vector3(holdSide, holdHeight, 0);

        transform.position = child.position + (Quaternion.Euler(0, targetYRotation, 0) * new  Vector3(holdSide, holdHeight, 0));

        transform.rotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);



	
	}
}
