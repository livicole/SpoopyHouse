﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CursorController : MonoBehaviour {

    [SerializeField]
    Camera ghostCam;

    [SerializeField]
    float speed;

    [SerializeField]
    float border;

    Transform child;

    [SerializeField]
    Transform spawn;

    [SerializeField]
    Transform detectionSphere;

    private GameObject gridBase;

    //1 is actions, 2 is Passive, 3 is Active, 4 is Environmental
    enum inputModes { Actions, Passive, Active, Environmental };
    private inputModes inputMode;

    //3 of the 4 categories of placeable objects. Last one, interactions, does not need a list of objects.

    //Dpad Right are actions
    [SerializeField]
    List<Transform> Actions;
    private bool holding = false, destroyable = false;
    public Transform holdingObject;
    private int previousLayer;
    public Vector3 targetLocation;
    public float angelCount = 0;
    public float inverterCount = 0;

    //Dpad Up
    [SerializeField]
    List<Transform> PassiveObjects;
    
    //Dpad Down
    [SerializeField]
    List<Transform> ActiveObjects;

    //Dpad Left
    [SerializeField]
    List<Transform> EnvironmentalEffects;

	public Image BButton, AButton;
	public Sprite bButtonUp, bButtonDown, aButtonUp, aButtonDown;



	// Use this for initialization
	void Start () {
        child = GameObject.Find("ChildPlayer").transform;
        
	}

    // Update is called once per frame
    void Update() {
        gridBase = GameObject.Find("GridBase");

        float inputX = Input.GetAxis("HorizontalMovement2");
        float inputY = Input.GetAxis("VerticalMovement2");
        Vector3 moveVector = new Vector3(inputX, -inputY, 0).normalized * speed;
        float minY = Screen.height / 2 + border;
        float maxY = Screen.height - border;
        float minX = border;
        float maxX = Screen.width - border;
        Vector3 newPos = GetComponent<RectTransform>().position + moveVector;
        newPos = new Vector3(Mathf.Clamp(newPos.x, minX, maxX),
                            Mathf.Clamp(newPos.y, minY, maxY),
                            0);
        GetComponent<RectTransform>().position = newPos;


        float dpadX = Input.GetAxis("Ghost DPad X");
        float dpadY = Input.GetAxis("Ghost DPad Y");
        //Debug.Log("DPadX: " + dpadX + " DPadY: " + dpadY);

        //Left on DPad
        if (dpadX < 0)
        {
            inputMode = inputModes.Environmental;
        }
        //Right on DPad
        else if (dpadX > 0)
        {
            inputMode = inputModes.Actions;
        }
        //Down on DPad
        if (dpadY < 0)
        {
            Debug.Log("active mode");
            inputMode = inputModes.Active;
        }
        //Up on DPad
        else if(dpadY > 0)
        {
            inputMode = inputModes.Passive;
        }

        int layermask = 1 << 5; layermask = ~layermask; // Ignoring UI layer      
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        //Debug.Log("Screen point: " + screenPoint);
        // Vector3 directionToPoint = ghostCam.ScreenPointToRay(screenPoint).direction.normalized;
        Ray verticalRay = ghostCam.ScreenPointToRay(screenPoint);
        //Ray verticalRay = ghostCam.ScreenPointToRay(screenPoint);
        RaycastHit verticalRayHit = new RaycastHit();
        //Debug.DrawRay(transform.position, verticalRay.direction * 10000f, Color.red);

        Transform detectedObj = detectionSphere.GetComponent<DetectionSphereController>().detectedObject;
        if(Physics.Raycast(verticalRay, out verticalRayHit, 100000f, layermask))
        {
            targetLocation = verticalRayHit.point;
            //Debug.Log(targetLocation);
            
        }
        //Debug.Log("Input Mode: " + inputMode);

        if(holdingObject != null)
        {
            if (holdingObject.FindChild("ChildPlayer") == null)
            {
                if (Input.GetButtonDown("LeftBumper"))
                {
                    //Layer 14 is gridlocked items i.e. rooms
                    if (holdingObject.gameObject.layer == 14)
                    {
                        holdingObject.transform.eulerAngles += new Vector3(0, -90, 0);
                        holdingObject.GetComponent<GridLocker>().UpdateCoordinates(-90);
                        //holdingObject.GetChild(0).transform.eulerAngles += new Vector3(0, -90, 0);

                    }
                    else
                    {
                        holdingObject.eulerAngles += new Vector3(0, -90, 0);

                    }

                }
                if (Input.GetButtonDown("RightBumper"))
                {
                    if (holdingObject.gameObject.layer == 14)
                    {
                        holdingObject.transform.eulerAngles += new Vector3(0, 90, 0);
                        holdingObject.GetComponent<GridLocker>().UpdateCoordinates(90);
                        //holdingObject.GetChild(0).transform.eulerAngles += new Vector3(0, 90, 0);
                    }
                    else
                    {
                        holdingObject.eulerAngles += new Vector3(0, 90, 0);
                    }
                }
            }
        }

        //Debug.Log("Input Mode: " + inputMode);

        Debug.DrawRay(transform.position, verticalRay.direction * 1000f, Color.red);

        //If in Actions mode
        if (inputMode == inputModes.Actions)
        {
			if (Input.GetButtonDown ("Ghost Button A")) {
				//AButton.sprite = aButtonDown;
				if (!handleHolding ()) {
					if (!holding && holdingObject == null) {
						if (detectedObj != null && detectedObj.gameObject.layer != 14) {
							Debug.Log ("Pickup");
							holdingObject = detectedObj;
                            previousLayer = holdingObject.gameObject.layer;
                            holdingObject.gameObject.layer = 5;
							if (holdingObject.GetComponent<Rigidbody> () != null) {
								holdingObject.GetComponent<Rigidbody> ().isKinematic = true;
							}
							holdingObject.transform.parent = detectionSphere.transform;
							holding = true;
							destroyable = false;
						}
					}
				} else {
					//AButton.sprite = aButtonUp;
				}
			} 

            //Smash things around. Currently works kind of like a wrecking ball.
            if (Input.GetButton("Ghost Button B"))
            {
				BButton.sprite = bButtonDown;
                if (!handleHolding())
                {
                    if (Physics.Raycast(verticalRay, out verticalRayHit, 100f, layermask))
                    {
                        targetLocation = verticalRayHit.point;
                        detectionSphere.GetComponent<DetectionSphereController>().moving = true;
                    }
                }
            }
            else if (Input.GetButtonUp("Ghost Button B"))
            {
				BButton.sprite = bButtonUp;
                detectionSphere.GetComponent<DetectionSphereController>().moving = false;
            }

            if(Input.GetButtonDown("Ghost Button X"))
            {
                if(!handleHolding())
                {
                    if (Physics.Raycast(verticalRay, out verticalRayHit, 100f, layermask))
                    {
                        if (verticalRayHit.collider.gameObject.layer == 10 && angelCount <= 0)
                        {
                            Instantiate(Actions[2], verticalRayHit.point, Quaternion.identity);
                            angelCount++;
                        }
                    }
                }
            }
            if(Input.GetButton("Ghost Button Y"))
            {
                if(!handleHolding())
                {
                    if(Physics.Raycast(verticalRay, out verticalRayHit, 100f, layermask))
                    {
                        if(verticalRayHit.collider.gameObject.layer == 10 && inverterCount <= 0)
                        {
                            Instantiate(Actions[3], verticalRayHit.point, Quaternion.identity);
                            inverterCount++;
                        }
                    }
                }
            }

        }
        //If in Passive mode
        else if (inputMode == inputModes.Passive)
        {
            if (Input.GetButton("Ghost Button A"))
            {

            }

            if (Input.GetButton("Ghost Button B"))
            {

            }

            if (Input.GetButton("Ghost Button X"))
            {

            }

            if (Input.GetButton("Ghost Button Y"))
            {

            }
        }
        //If in Active mode
        else if (inputMode == inputModes.Active)
        {
            //Debug.Log("InputMode");
            if (Input.GetButtonDown("Ghost Button A"))
            {
                //Debug.Log("A BUTTON");
                if (Physics.Raycast(verticalRay, out verticalRayHit, 10000f, layermask))
                {
                    Debug.Log("Hitting object: " + verticalRayHit.collider.gameObject.name);
                    
                    //Layer 14 is gridlocked.
                    if(verticalRayHit.collider.gameObject.layer == 14)
                    {
                        Debug.Log("Correct");
                        holdingObject = verticalRayHit.collider.gameObject.transform.parent;
                        Debug.Log(holdingObject);
                        //holdingObject.gameObject.layer = 5;
                        //holdingObject.GetComponent<GridLocker>().locked = false;
                        holdingObject.parent = detectionSphere;
                        detectionSphere.GetComponent<DetectionSphereController>().invisGrid = false;
                        holdingObject.transform.GetComponent<GridLocker>().locked = false;
                        holdingObject.transform.GetComponent<GridLocker>().ClearOldBlocks();
                        
                    }
                }
            }
            else if(Input.GetButtonUp("Ghost Button A"))
            {
                if(holdingObject != null)
                {
                    holdingObject.parent = null;
                    //holdingObject.gameObject.layer = 14; 
                    //holdingObject.GetComponent<GridLocker>().locked = true;
                    holdingObject.transform.GetComponent<GridLocker>().locked = true;
                    holdingObject.transform.GetComponent<GridLocker>().UpdateNewBlocks();
                    detectionSphere.GetComponent<DetectionSphereController>().invisGrid = true;
                    holdingObject = null;
                }
            }

            if (Input.GetButton("Ghost Button B"))
            {

            }

            if (Input.GetButton("Ghost Button X"))
            {

            }

            if (Input.GetButton("Ghost Button Y"))
            {

            }
        }
        //If in Environmental mode
        else if (inputMode == inputModes.Environmental)
        {
            if (Input.GetButton("Ghost Button A"))
            {

            }

            if (Input.GetButton("Ghost Button B"))
            {

            }

            if (Input.GetButton("Ghost Button X"))
            {

            }

            if (Input.GetButton("Ghost Button Y"))
            {

            }
        }


        /** Old Code
        if (Input.GetButtonDown("Ghost Button A"))
        {
           

            
            Debug.Log("Detected");
            Ray verticalRay = new Ray(transform.position, Vector3.down * 100f);
            Debug.DrawRay(transform.position, Vector3.down * 100f, Color.red);
            RaycastHit verticalRayHit = new RaycastHit();

            if (Physics.Raycast(verticalRay, out verticalRayHit, 100f))
            {
                if (verticalRayHit.collider.gameObject.layer == 10)
                {
                    Vector3 spawnPosition = new Vector3(verticalRayHit.point.x, 1, verticalRayHit.point.z);
                    Instantiate(spawn, spawnPosition, Quaternion.identity);
                    child.GetComponent<FearInfo>().fearLevel += 1;
                }
            }
        }*/
	}

   public bool handleHolding()
   {
        if (holding && destroyable)
        {
            Destroy(holdingObject.gameObject);
            holdingObject = null;
            holding = false;
            Debug.Log("Despawning");
            return true;
        }

        if (holding && !destroyable)
        {
            holdingObject.transform.parent = null;
            holdingObject.gameObject.layer = previousLayer;
            previousLayer = 1;
            holdingObject = null;
            holding = false;
            Debug.Log("Dropping");
            return true;
        }
        return false;
   }

    public void PushBack(Collider other, float strength)
    {
        Debug.Log("PushBack(" + other + ")");
    
        Vector3[] cardinals;
        cardinals = new Vector3[4];
        targetLocation = new Vector3(0, 0, 0);
        cardinals[0] = new Vector3(0, 0, 1); //Up direction
        cardinals[1] = new Vector3(1, 0, 0); //Right direction
        cardinals[2] = new Vector3(0, 0, -1); // Down direction
        cardinals[3] = new Vector3(-1, 0, 0);// Left direction
        int direction = 0; Vector3 currentDifference = new Vector3(999, 999, 999);
        Vector3 directionToRoom = -(other.gameObject.transform.position - transform.position).normalized;
        Debug.Log("directionToRoom: " + directionToRoom);

        for (int i = 0; i < 4; i++)
        {
            Vector3 difference = directionToRoom - cardinals[i];
            //Debug.Log("Difference: " + difference);
            if (difference.magnitude < currentDifference.magnitude)
            {
                currentDifference = difference;
                direction = i;
            }
        }

        if (direction == 0)
        {
            transform.position += cardinals[0] * strength;
        }
        else if (direction == 1)
        {
            transform.position += cardinals[1] * strength;
        }
        else if (direction == 2)
        {
            transform.position += cardinals[2] * strength;
        }
        else if (direction == 3)
        {
            transform.position += cardinals[3] * strength;
        }
    }


}