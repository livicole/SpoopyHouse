using UnityEngine;
using System.Collections;

public class GhostCameraController : MonoBehaviour {

    [SerializeField]
    public float panSpeed;

    [SerializeField]
    float zoomSpeed;

    [SerializeField]
    float zoomMin, zoomMax, xMin, xMax, zMin, zMax;

    Vector3 zoomedOutCenter;
    public enum CamZoom { Close, Mid, Far };
    CamZoom camZoomMode = CamZoom.Far;
    float midZoom, closeZoom;
    float timer, zoomCD = 0.5f;
    bool isLeftTriggerInUse = false, isRightTriggerInUse = false;
    Vector3 playerPosition;
    

	// Use this for initialization
	void Start () {
        GridInfo gridInfo = GameObject.Find("GridBase").GetComponent<GridInfo>();
        zoomedOutCenter = new Vector3((gridInfo.gridMax / 2 + 0.5f) * gridInfo.blockLength, zoomMax, (gridInfo.gridMax / 2 + 1) * gridInfo.blockLength);
        xMin = gridInfo.blockLength; zMin = gridInfo.blockLength;
        xMax = (gridInfo.gridSize + 1) * gridInfo.blockLength;
        zMax = (gridInfo.gridSize + 1) * gridInfo.blockLength;

        //Debug.Log(zoomedOutCenter);
        transform.position = zoomedOutCenter;
        midZoom = (zoomMax - zoomMin) / 2 + zoomMin;
        closeZoom = zoomMin;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().gameIsLive)
        {
            float inputX = Input.GetAxis("HorizontalCamera2");
            float inputY = -Input.GetAxis("VerticalCamera2");

            float left = -Input.GetAxis("GhostLeftTrigger"); //-1 -> 0
            float right = Input.GetAxis("GhostRightTrigger"); //0 -> 1
                                                              //float combination = left + right;
                                                              //inputX = Input.GetAxis("LeftTrigger");
                                                              //float inputZ = combination;
                                                              //Debug.Log(inputZ);
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                if (Input.GetAxisRaw("GhostLeftTrigger") != 0)
                {
                    if (!isLeftTriggerInUse)
                    {
                        Debug.Log("Use left trigger");
                        isLeftTriggerInUse = true;
                        camZoomMode -= 1;
                        if (camZoomMode < CamZoom.Close)
                        {
                            camZoomMode = CamZoom.Close;
                        }
                        SetZoom(camZoomMode);
                    }
                }
                if (Input.GetAxisRaw("GhostLeftTrigger") == 0)
                {
                    isLeftTriggerInUse = false;
                }
                if (Input.GetAxisRaw("GhostRightTrigger") != 0)
                {
                    if (!isRightTriggerInUse)
                    {
                        Debug.Log("Use right trigger");
                        isRightTriggerInUse = true;
                        camZoomMode += 1;
                        if (camZoomMode > CamZoom.Far)
                        {
                            camZoomMode = CamZoom.Far;
                        }
                        SetZoom(camZoomMode);
                    }
                }
                if (Input.GetAxisRaw("GhostRightTrigger") == 0)
                {
                    isRightTriggerInUse = false;
                }
            }

            if (Input.GetButtonDown("Ghost Left Stick Click"))
            {
                Debug.Log("Left Stick Click");
                playerPosition = GameObject.Find("ChildPlayer").transform.position;
                camZoomMode = CamZoom.Close;
                SetZoom(camZoomMode);
                transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z + 2.5f);

            }


            Vector3 movementVector = new Vector3(inputX, 0, inputY);
            movementVector = movementVector.normalized * panSpeed;

            /* Vector3 zoomVector = new Vector3(0, 
                 inputZ * zoomSpeed
                 , 0);*/

            Vector3 totalMovement = movementVector;// + zoomVector;

            Vector3 newPosition = transform.position + totalMovement * Time.deltaTime;
            newPosition = new Vector3(Mathf.Clamp(newPosition.x, xMin, xMax),
                Mathf.Clamp(newPosition.y, zoomMin, zoomMax),
                Mathf.Clamp(newPosition.z, zMin, zMax));
            transform.position = newPosition;

        }

    }

    public bool SetZoom(CamZoom zoom)
    {
        if(zoom == CamZoom.Far)
        {
            transform.position = zoomedOutCenter;
            panSpeed = 50f;
            return true;
        }
        else if(zoom == CamZoom.Mid)
        {
            transform.position = new Vector3(transform.position.x, midZoom, transform.position.z);
            panSpeed = 30f;
            return true;
        }
        else if(zoom == CamZoom.Close)
        {
            transform.position = new Vector3(transform.position.x, closeZoom, transform.position.z);
            panSpeed = 10f;
            return true;
        }
        return false;
    }
}
