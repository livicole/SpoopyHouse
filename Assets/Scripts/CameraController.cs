using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    float turningSpeed;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float camYAxis = Input.GetAxis("VerticalCamera");

        transform.Rotate(new Vector3(turningSpeed * camYAxis, 0, 0));
    }
}
