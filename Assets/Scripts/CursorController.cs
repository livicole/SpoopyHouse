using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class CursorController : MonoBehaviour {

    [SerializeField]
    float speed;

    Transform child;

    [SerializeField]
    Transform spawn;

    [SerializeField]
    Transform detectionSphere;

    //1 is actions, 2 is Passive, 3 is Active, 4 is Environmental
    enum inputModes { Actions, Passive, Active, Environmental };
    private inputModes inputMode;

    //3 of the 4 categories of placeable objects. Last one, interactions, does not need a list of objects.

    //Dpad Right are actions
    [SerializeField]
    List<Transform> Actions;
    private bool holding = false, destroyable = false;
    private Transform holdingObject;
    public Vector3 targetLocation;

    //Dpad Up
    [SerializeField]
    List<Transform> PassiveObjects;
    
    //Dpad Down
    [SerializeField]
    List<Transform> ActiveObjects;

    //Dpad Left
    [SerializeField]
    List<Transform> EnvironmentalEffects;


    public Transform toySpawnLocation;
    public GameObject[] ghostToys = new GameObject[3];
    int activeToyIndex;
    public Text activeToyText;



	// Use this for initialization
	void Start () {
        activeToyText.text = ghostToys[0].name;
        child = GameObject.Find("ChildPlayer").transform;
        toySpawnLocation = GameObject.Find("ToySpawnLocation").transform;
	}

    // Update is called once per frame
    void Update() {



        float inputX = Input.GetAxis("HorizontalMovement2");
        float inputY = Input.GetAxis("VerticalMovement2");

        GetComponent<CharacterController>().Move(new Vector3(inputX, 0, -inputY) * speed);

        float dpadX = Input.GetAxis("Ghost DPad X");
        float dpadY = Input.GetAxis("Ghost DPad Y");

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
            inputMode = inputModes.Active;
        }
        //Up on DPad
        else if(dpadY > 0)
        {
            inputMode = inputModes.Passive;
        }

        int layermask = 1 << 5; layermask = ~layermask; // Ignoring UI layer
        Ray verticalRay = new Ray(transform.position, Vector3.down * 100f);
        RaycastHit verticalRayHit = new RaycastHit();

        Transform detectedObj = detectionSphere.GetComponent<DetectionSphereController>().detectedObject;

        //If in Actions mode
        if (inputMode == inputModes.Actions)
        {
            if (Input.GetButtonDown("Ghost Button A"))
            {
                if (!handleHolding())
                {
                    if (!holding && holdingObject == null)
                    {
                        if(detectedObj != null)
                        {
                            Debug.Log("Pickup");
                            holdingObject = detectedObj;
                            if(holdingObject.GetComponent<Rigidbody>() != null)
                            { holdingObject.GetComponent<Rigidbody>().isKinematic = true; }
                            holdingObject.transform.parent = detectionSphere.transform;
                            holding = true;
                            destroyable = false;
                        }
                    }
                }
            }

            //Smash things around. Currently works kind of like a wrecking ball.
            if (Input.GetButton("Ghost Button B"))
            {
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
                detectionSphere.GetComponent<DetectionSphereController>().moving = false;
            }

            if(Input.GetButtonDown("Ghost Button X"))
            {
                Debug.Log("I pressed x");
                Instantiate(ghostToys[activeToyIndex], toySpawnLocation.position, Quaternion.identity);

            }

            if(Input.GetButtonDown("Ghost Button Y"))
            {
                Debug.Log("i pressed y");
                activeToyIndex = (activeToyIndex+1) % 3;
                activeToyText.text = ghostToys[activeToyIndex].name;
                Debug.Log(activeToyIndex);

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
            holdingObject = null;
            holding = false;
            Debug.Log("Dropping");
            return true;
        }
        return false;
   }

  
}
