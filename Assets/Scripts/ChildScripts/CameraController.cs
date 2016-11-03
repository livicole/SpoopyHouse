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
    
        {
            float camYAxis = Input.GetAxis("VerticalCamera");
            float rotationY = camYAxis * -turningSpeed;
            rotationY = Mathf.Clamp(rotationY, 0, 360);
            //Debug.Log(Mathf.Abs(transform.eulerAngles.x));
            Vector3 rotation = transform.eulerAngles + new Vector3(turningSpeed * camYAxis, 0, 0);

            rotation.x = Mathf.Clamp(rotation.x, 0, 1080);

            transform.eulerAngles = rotation;
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
}
