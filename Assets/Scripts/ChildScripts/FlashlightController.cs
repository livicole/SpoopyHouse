﻿using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour {

    /*
    [SerializeField]
    float flashLightSpeed;
    **/
    [SerializeField]
    float minimumRange;

    [SerializeField]
    public float maximumRange;

    [SerializeField]
    float minimumAngle;

    [SerializeField]
    float maximumAngle;

    [SerializeField]
    float timeFromMinToMax;

    private float inputX;
	private Animator controlAnimator;
	public Animator dresserAnimator;
    private Light flashlight;
    private float angleChange, rangeChange;
	public bool drawerOpen;

    // Use this for initialization
    void Start() {

        controlAnimator = GetComponent<Animator>();
        flashlight = GetComponent<Light>();

        angleChange = (maximumAngle - minimumAngle) / timeFromMinToMax;
        rangeChange = (maximumRange - minimumRange) / timeFromMinToMax;
        //Debug.Log("Range :" + rangeChange);
 
    }
    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD:Assets/Scripts/FlashlightController.cs
		Ray drawerRay = new Ray (transform.position, transform.forward);
		RaycastHit rayHitInfo = new RaycastHit ();
		if (Physics.Raycast (drawerRay, out rayHitInfo, 1000f)) {
			Debug.DrawRay (transform.position, transform.forward * 1000f, Color.blue);
			Debug.Log (rayHitInfo.collider.name);
			if (rayHitInfo.transform.tag == "Drawer") {
				Debug.Log ("it hit");
				if (Input.GetButton ("Ghost Button A") && drawerOpen == false) {
					Debug.Log ("open drawer");
					dresserAnimator.SetBool ("DrawerIsOpen", true);
					dresserAnimator.SetBool ("DrawerIsClosed", false);
					drawerOpen = true;
				}
				if (Input.GetButton ("Ghost Button A") && drawerOpen == true) {
					Debug.Log ("close drawer");
					dresserAnimator.SetBool ("DrawerIsOpen", false);
					dresserAnimator.SetBool ("DrawerIsClosed", true);
					drawerOpen = false;
				}
			}
		}

        Debug.Log("Trigger: " + Input.GetAxis("LeftTrigger"));
=======
        //Debug.Log("Trigger: " + Input.GetAxis("LeftTrigger"));
>>>>>>> 28218bc79c956be422c1b7e1bc30c784a23f4b20:Assets/Scripts/ChildScripts/FlashlightController.cs
        float left = -Input.GetAxis("LeftTrigger"); //-1 -> 0
        float right = Input.GetAxis("RightTrigger"); //0 -> 1
        float combination = left + right;
        //inputX = Input.GetAxis("LeftTrigger");
        inputX = (combination + 1) / 2;
        if(inputX == 1)
        {
            inputX = 0.99f;
        }

        //Debug.Log("Input X: " + (double)(inputX));
        controlAnimator.SetTime((double)(inputX));


        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Animator>().StartPlayback();
            //GetComponent<Animator>().Play("ControlAnimation", 0);
        }
        
        //Increased range and narrower beam.
        if (Input.GetButton("LeftBumper"))
        {
            if(flashlight.range <= maximumRange)
            {
                flashlight.range += rangeChange * Time.deltaTime;
                flashlight.spotAngle -= angleChange * Time.deltaTime;
            }
        }

        //Decreased range and wider beam.
        if (Input.GetButton("RightBumper"))
        {
            if (flashlight.range >= minimumRange)
            {
                flashlight.range -= rangeChange * Time.deltaTime;
                flashlight.spotAngle += angleChange * Time.deltaTime;
            }
        }
    }

}