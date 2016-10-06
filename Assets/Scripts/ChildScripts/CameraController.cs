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
        Debug.Log(Mathf.Abs(transform.eulerAngles.x));
        transform.Rotate(new Vector3(turningSpeed * camYAxis, 0, 0));
        /**
        if (Mathf.Abs(transform.localEulerAngles.x) > 50)
        {
            transform.localEulerAngles = new Vector3(50, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        else if (Mathf.Abs(transform.localEulerAngles.x) < 300)
        {
            transform.localEulerAngles = new Vector3(300, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }**/
    }
}
