using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CursorController : MonoBehaviour {

    [SerializeField]
    LayerMask inRoomLayerMask;

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
    /**
    [SerializeField]
    List<Transform> Actions;**/
    private bool holding = false, destroyable = false;
    [HideInInspector]
    public Transform holdingObject;
    private int previousLayer;
    [HideInInspector]
    public Vector3 targetLocation;
    [HideInInspector]
    public float angelCount = 0;
    [HideInInspector]
    public float inverterCount = 0;

    List<float> toyCooldowns = new List<float>();

    public Text toyCDText;

    [SerializeField]
    List<ToyIndex> ghostToys;
    private int selector;
    private bool isYAxisInUse = false;

    public List<Sprite> toySprites= new List<Sprite>();
    public List<Sprite> toySpritesShaded = new List<Sprite>();
    private Image selectedToySprite;
    private Image previousToySprite;
    private Image nextToySprite;

    private float rotateTimer;
    [SerializeField]
    float rotateCD;

    [SerializeField]
    float panEdgeSize;

    /** Redundant lists.
    //Dpad Up
    [SerializeField]
    List<Transform> PassiveObjects;
    
    //Dpad Down
    [SerializeField]
    List<Transform> ActiveObjects;

    //Dpad Left
    [SerializeField]
    List<Transform> EnvironmentalEffects;
    **/

	public Image BButton, AButton;
	public Image bButtonUp, bButtonDown, aButtonUp, aButtonDown;
    private bool holdingRoom = false;



	// Use this for initialization
	void Start () {
        child = GameObject.Find("ChildPlayer").transform;

        selectedToySprite = GameObject.Find("SelectedToySprite").GetComponent<Image>();
        previousToySprite = GameObject.Find("PreviousToySprite").GetComponent<Image>();
        nextToySprite = GameObject.Find("NextToySprite").GetComponent<Image>();

        //initialize cooldown list
    }

    // Update is called once per frame
    void Update() {
        //show cd of toy
        if (ghostToys[selector].timer > 0)
        {
            toyCDText.text = Mathf.Floor(ghostToys[selector].timer).ToString();
        }
        else
        {
            toyCDText.text = "";
        }


        //tick cooldown timers
        for (int i=0; i<ghostToys.Count; i++)
        {
            if (ghostToys[i].timer > 0)
            {
                ghostToys[i].timer -= Time.deltaTime;
            }
        }

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
        GhostCameraController ghostCamController = ghostCam.GetComponent<GhostCameraController>();
        
        if (newPos.x <= minX + panEdgeSize)
        {
            if(inputX < 0)
            {
                ghostCam.transform.position = new Vector3(ghostCam.transform.position.x - (ghostCamController.panSpeed * Time.deltaTime),
               ghostCam.transform.position.y,
               ghostCam.transform.position.z);
            }
           
        }
        else if(newPos.x >= maxX - panEdgeSize)
        {
            if(inputX > 0)
            {
                ghostCam.transform.position = new Vector3(ghostCam.transform.position.x + (ghostCamController.panSpeed * Time.deltaTime),
             ghostCam.transform.position.y,
             ghostCam.transform.position.z);
            }   
        }

        if (newPos.y <= minY + panEdgeSize)
        {
            if (inputY > 0)
            {
                ghostCam.transform.position = new Vector3(ghostCam.transform.position.x,
                    ghostCam.transform.position.y,
                    ghostCam.transform.position.z - (ghostCamController.panSpeed * Time.deltaTime));
            }
        }
        else if (newPos.y >= maxY - panEdgeSize)
        {
            if(inputY < 0)
            {
                  ghostCam.transform.position = new Vector3(ghostCam.transform.position.x,
                          ghostCam.transform.position.y,
                         ghostCam.transform.position.z + (ghostCamController.panSpeed * Time.deltaTime));
            }
        }


        GetComponent<RectTransform>().position = newPos;


        //float dpadX = Input.GetAxisRaw("Ghost DPad X");
        //Code below does two things. 
        //1. Makes the DPad act as a button
        //2. Modify the selector index to cycle through our objects.
        if(Input.GetAxisRaw("Ghost DPad Y") != 0)
        {
            if (!isYAxisInUse)
            {
                //Press up
                if(Input.GetAxisRaw("Ghost DPad Y") > 0)
                {
                    selector = (selector + 1) % ghostToys.Count;
                }
                //Press down
                else if(Input.GetAxisRaw("Ghost DPad Y") < 0)
                {
                    if(selector != 0)
                    {
                        selector--;
                    }
                    else
                    {
                        selector = ghostToys.Count - 1;
                    }
                    
                }
                

                isYAxisInUse = true;
            }
        }
        if(Input.GetAxisRaw("Ghost DPad Y") == 0)
        {
            isYAxisInUse = false;
        }
        //Debug.Log(selector);
        //Debug.Log("DPadX: " + dpadX + " DPadY: " + dpadY);

        //Left on DPad
        /**Redundant inputmode Select
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
        }**/

        //Display the selected object on screen.
        /*selectedObjectText.text = ghostToys[selector].toy.name;
        nextObjectText.text = ghostToys[(selector+1)%6].toy.name;
        if (selector == 0)
        {
            previousObjectText.text = ghostToys[ghostToys.Count - 1].toy.name;
        }
        else
        {
            previousObjectText.text = ghostToys[selector - 1].toy.name;
        }*/

        selectedToySprite.sprite = toySprites[selector];
        nextToySprite.sprite = toySpritesShaded[(selector + 1) % 6];
        if (selector == 0)
        {
            previousToySprite.sprite = toySpritesShaded[toySprites.Count - 1];
        }
        else
        {
            previousToySprite.sprite = toySpritesShaded[selector - 1];
        }

        //Some preliminary code to set up the ability to drop objects.
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
        //Moving rooms using button A
     
        if (Input.GetButtonDown("Ghost Button A"))
        {
            //Debug.Log("A BUTTON");
            if (Physics.Raycast(verticalRay, out verticalRayHit, 10000f, layermask))
            {
                Debug.Log("Hitting object: " + verticalRayHit.collider.gameObject.name);

                //Layer 14 is gridlocked.
                if (verticalRayHit.collider.gameObject.layer == 14)
                {
                    holdingRoom = true;
                    holdingObject = verticalRayHit.collider.gameObject.transform.parent;
                    Debug.Log(holdingObject);
                    /* Old pickup code.
                    Debug.Log("Correct");
                    holdingObject = verticalRayHit.collider.gameObject.transform.parent;
                    Debug.Log(holdingObject);
                    //holdingObject.gameObject.layer = 5;
                    //holdingObject.GetComponent<GridLocker>().locked = false;
                    holdingObject.parent = detectionSphere;
                    detectionSphere.GetComponent<DetectionSphereController>().invisGrid = false;
                    holdingObject.transform.GetComponent<GridLocker>().locked = false;
                    holdingObject.transform.GetComponent<GridLocker>().ClearOldBlocks();
                    */
                }
            }
        }
        else if (Input.GetButtonUp("Ghost Button A"))
        {
            if (holdingObject != null)
            {
                foreach (Transform myDoor in holdingObject.transform.GetChild(0))
                {
                    if (myDoor.tag == "Door")
                    {

                        myDoor.GetComponent<DoorScript>().ResetDoors();
                    }
                }
                holdingRoom = false;
                holdingObject = null;

                /*
                holdingObject.parent = null;
                //holdingObject.gameObject.layer = 14; 
                //holdingObject.GetComponent<GridLocker>().locked = true;
                holdingObject.transform.GetComponent<GridLocker>().locked = true;
                holdingObject.transform.GetComponent<GridLocker>().UpdateNewBlocks();
                detectionSphere.GetComponent<DetectionSphereController>().invisGrid = true;
                holdingObject = null;
                */
            }
        }

        if (holdingRoom)
        {
            //Debug.Log("Holding the room.");
            GetComponent<Image>().enabled = false;
            Vector3 directionToMove;
            //Read in left stick input. If it moves record it. If both X and Y are used, then set Y to 0 and assume using X.
            float roundedX = inputX, roundedY= -inputY;
            //Debug.Log("RoundedX: " + roundedX + " RoundedY: " + roundedY);
            if (roundedX > 0) { roundedX = 1; } else if (roundedX < 0) { roundedX = -1; }
            if (roundedY > 0) { roundedY = 1; } else if (roundedY < 0) { roundedY = -1; }
            if(roundedX != 0  && roundedY != 0)
            {
                roundedY = 0;
            }
            else if(roundedX != 0 || roundedY != 0)
            {
                directionToMove = new Vector3(roundedX, 0, roundedY);
                Debug.Log("Moving the room! Direction: " + directionToMove);
                holdingObject.GetComponent<GridLocker>().MoveDirection(directionToMove);
            }
            if (rotateTimer >= rotateCD)
            {
                if (Input.GetButtonDown("GhostLeftBumper"))
                {
                   
                    holdingObject.GetComponent<GridLocker>().ClearOldBlocks();
                    holdingObject.GetComponent<GridLocker>().UpdateCoordinates(-90);
                    holdingObject.GetComponent<GridLocker>().UpdateNewBlocks();
                        //holdingObject.GetChild(0).transform.eulerAngles += new Vector3(0, -90, 0);
                        rotateTimer = 0;
                }
                if (Input.GetButtonDown("GhostRightBumper"))
                {
                   
                    holdingObject.GetComponent<GridLocker>().ClearOldBlocks();
                    holdingObject.GetComponent<GridLocker>().UpdateCoordinates(90);
                    holdingObject.GetComponent<GridLocker>().UpdateNewBlocks();
                    //holdingObject.GetChild(0).transform.eulerAngles += new Vector3(0, 90, 0);
                    rotateTimer = 0;
                }
            }
        }
        else
        {
            GetComponent<Image>().enabled = true;
        }

        //Smashing things with button B
        if (Input.GetButton("Ghost Button B"))
        {
            //BButton.sprite = bButtonDown;
            if (!handleHolding())
            {
                if (Physics.Raycast(verticalRay, out verticalRayHit, 100f, inRoomLayerMask))
                {
                    targetLocation = verticalRayHit.point;
                    detectionSphere.GetComponent<DetectionSphereController>().moving = true;
                }
            }
        }
        else if (Input.GetButtonUp("Ghost Button B"))
        {
            //BButton.sprite = bButtonUp;
            detectionSphere.GetComponent<DetectionSphereController>().moving = false;
        }
        //Using toys with button X
        if(Input.GetButtonDown("Ghost Button X"))
        {
            if (!handleHolding())
            {
                if (Physics.Raycast(verticalRay, out verticalRayHit, 1000f, inRoomLayerMask))
                {
                    if (ghostToys[selector].timer <= 0)
                    {
                        Vector3 targetSpawn = verticalRayHit.point + new Vector3(0, 2, 0);
                        Instantiate(ghostToys[selector].toy, targetSpawn, Quaternion.identity);
                        ghostToys[selector].timer = ghostToys[selector].cooldown;
                    }
                    else {
                        Debug.Log("Cooldown: " + ghostToys[selector].timer + " on " + ghostToys[selector].toy.name);
                    }

                }
            }
        }

        //Rotating rooms while held.
        //Some preliminary setup to add a cooldown to rotate 
        if(rotateTimer < rotateCD)
        {
            rotateTimer += Time.deltaTime;
        }


        //Debug.Log("Input Mode: " + inputMode);

        Debug.DrawRay(transform.position, verticalRay.direction * 1000f, Color.red);




        /** Old code for DPad mode selection.
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
                    if (Physics.Raycast(verticalRay, out verticalRayHit, 100f, inRoomLayerMask))
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
                    if (Physics.Raycast(verticalRay, out verticalRayHit, 100f, inRoomLayerMask))
                    {
                        Debug.Log(verticalRayHit.collider.name);
                        if (verticalRayHit.collider.gameObject.layer == 10 && angelCount <= 0)
                        {
                            Debug.Log("Angel spawn.");
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
                    if(Physics.Raycast(verticalRay, out verticalRayHit, 100f, inRoomLayerMask))
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
        }**/


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


[Serializable]
public class ToyIndex
{
    public Transform toy;
    public float cooldown;
    public float timer = 0;
}
