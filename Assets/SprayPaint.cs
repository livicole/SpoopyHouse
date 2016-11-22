using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SprayPaint : MonoBehaviour {

    GameObject theCamera;
    public Transform bluePaint, greenPaint, redPaint, whitePaint;
    int paintsIndex = 0;
    List<Transform> paints = new List<Transform>();

	// Use this for initialization
	void Start () {
        theCamera = GameObject.Find("ChildCamera");
        paints.Add(bluePaint);
        paints.Add(greenPaint);
        paints.Add(redPaint);
        paints.Add(whitePaint);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("LeftBumper"))
        {
            paintsIndex = (paintsIndex + 1) % 4;
            Debug.Log(paints[paintsIndex]);
        }
        if (Input.GetButton("RightBumper"))
        {
            Ray sprayRay = new Ray(theCamera.transform.position, theCamera.transform.forward);
            RaycastHit sprayRayHit = new RaycastHit();
            if (Physics.Raycast(sprayRay, out sprayRayHit, 3f))
            {
                if (sprayRayHit.collider.tag == "Walls")
                {
                    Debug.Log(sprayRayHit.normal);
                    Vector3 rotationVector = Vector3.zero;
                    if (sprayRayHit.normal.x != 0)
                    {
                        rotationVector = new Vector3(0, 90f, 0);
                    }

                   

                    Instantiate(paints[paintsIndex], sprayRayHit.point - theCamera.transform.forward*.01f, Quaternion.Euler(rotationVector));
                }
            }
        }
    }
}
