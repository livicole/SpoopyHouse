using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    float sensitivity;

    float xRotation;

    private float currentXRotation, xRotationV, lookSmoothDamp;


    // Use this for initialization
    void Start () {
        xRotation = 0;
        xRotationV = 1;
        lookSmoothDamp = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
        {
           
            float camYAxis = Input.GetAxis("VerticalCamera");
            xRotation +=  camYAxis * sensitivity;

            xRotation = Mathf.Clamp(xRotation, -90, 90);
            Debug.Log(xRotation);
            currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);

            Vector3 rotation = new Vector3(currentXRotation, transform.localEulerAngles.y, 0);
            //Debug.Log(currentXRotation);
            //Debug.Log(Mathf.Abs(transform.eulerAngles.x));
            //Vector3 rotation = transform.eulerAngles + new Vector3(turningSpeed * camYAxis, 0, 0);

            //rotation.x = Mathf.Clamp(rotation.x, 0, 1080);

            transform.localRotation = Quaternion.Euler(rotation);
            
        
        }
    }
}
