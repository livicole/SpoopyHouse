using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GridLocker : MonoBehaviour {

    /**NOTE: In order to use this script certain steps need to be followed when making rooms.

    1.Create an empty gameobject and set to 0,0,0.
    2.Create a square box that will house all the smaller blocks that are in your room. 
            i.e. A room that like the W-Room needs a box that is 3(60x60) boxes wide and tall. Short-L Rooms need a 2x2 (40x40).
    3.Set the name of the box to Room Filler. Then disable it's mesh renderer. Leave the collider.
    4.Create boxes of size actual 20x20, and place them within the box, each taking up one of the 20x20 grid spaces in it.
    5.On the empty gameobject, add the script GridLocker. Under CoordinatesOccupied, set the size to the number of blocks in your invisible box.
    6.For each small box in the big box, put in their corresponding coordinates /20 for the grid coordinate. 
            i.e. A block at X: 20 and Z: 40 is the equivalent to coordinate 1, 0, 2. Note the Y value is always 0.
    7. Parent the big, invisible box to the empty object. Parent all small boxes inside the invisible box.

    **/



    //GameObject[] doors = new GameObject[30];

    public List<Transform> doors = new List<Transform>();
    public bool amIConnected;

    Transform gridBase;

    [SerializeField]
    public float height;

    [SerializeField]
    public Vector3 gridLocation;

    [SerializeField]
    float rotation;

    [SerializeField]
    public bool locked = true;

    [SerializeField]
    public int roomNumber;
        
   [SerializeField]
    public List<Vector3> coordinatesOccupied;

    private float gridXPosition, gridYPosition;
    private Vector3 currentLocation, adjustmentVector, originalRoomFillerPosition, currentRoomFillerPosition;
    private GridInfo gridInfo;
    private float blockLength;
    public float moveTick;
    private float moveCooldownTimer = 0;
    [HideInInspector]
    public float rotationY;
    public Vector3 previousConnectedPosition;
    public Quaternion previousConnectedRotation;
    public bool childLocked = false, moving = false;
    public List<Transform> connectedDoors;
    int counter;

    // Use this for initialization
    void Start () {

        //initialize door open bool list
        foreach(Transform myObject in transform.GetChild(0))
        {
            
            if (myObject.tag == "Door")
            {
                doors.Add(myObject);
            }
        }
        


        gridBase = GameObject.Find("GridBase").transform;
      
        currentLocation = CalculateGridToReal(gridLocation);
       
        originalRoomFillerPosition = transform.FindChild("RoomFiller").localPosition;

       
        gridInfo = gridBase.GetComponent<GridInfo>();
        blockLength = gridInfo.blockLength;
        Physics.IgnoreLayerCollision(12, 14, true);
        moveTick = gridInfo.moveTick;

        transform.position = CalculateGridToReal(gridLocation);
        rotationY = transform.eulerAngles.y;
        MoveOrigin(rotationY);

        if(gridInfo.usedGridBlocks == null)
        {
            gridInfo.InitList();
        }
        InitToGridInfo();

    }

    // Update is called once per framsdses
    void Update()
    {
        foreach (Transform door in doors)
        {
            if (door.GetComponent<DoorScript>().isConnected)
            {
                amIConnected = true;
                if (!moving)
                {
                    previousConnectedPosition = CalculateRealToGrid(transform.position);
                    previousConnectedRotation = transform.rotation;
                    
                }          
                break;
            }
            else
            {
                amIConnected = false;
            }
        }

        gridBase = GameObject.Find("GridBase").transform;
        gridInfo = gridBase.GetComponent<GridInfo>();
        if (moveCooldownTimer <= moveTick && moveCooldownTimer >= 0)
        {
            moveCooldownTimer -= Time.deltaTime;
        }
    }

    public void MoveOrigin(float newRotation)
    {

        //Move pivot on rotation.   
        float positiveMultiplier, negativeMultiplier;
        //Positive should be smaller.
        positiveMultiplier = 0.999f;//.0009995f;
        //Negative should be more negative;
        negativeMultiplier = 0.999f;//0.9990005f;
        //Debug.Log(rotationY);
        //Debug.Log("Rotation: " + rotationY);
        if (newRotation == 0 || newRotation == 360)
        {
            //Debug.Log("No rotation.");
            Vector3 newFillerPosition = originalRoomFillerPosition;
            //Debug.Log("Original: " + originalRoomFillerPosition);
            newFillerPosition = new Vector3(newFillerPosition.x * positiveMultiplier, newFillerPosition.y, newFillerPosition.z * positiveMultiplier);
            transform.FindChild("RoomFiller").localPosition = newFillerPosition;
        }
        else if (newRotation == 90)
        {
            //Debug.Log("detected rotation");
            Vector3 newFillerPosition = originalRoomFillerPosition;

            newFillerPosition = new Vector3(-newFillerPosition.x * negativeMultiplier, newFillerPosition.y, newFillerPosition.z * positiveMultiplier);
            transform.FindChild("RoomFiller").localPosition = newFillerPosition;
        }
        else if (newRotation == 180)
        {

            Vector3 newFillerPosition = originalRoomFillerPosition;
            newFillerPosition = new Vector3(-newFillerPosition.x * negativeMultiplier, newFillerPosition.y, -newFillerPosition.z * negativeMultiplier);
            transform.FindChild("RoomFiller").localPosition = newFillerPosition;
        }
        else if (newRotation == 270 || newRotation == -90)
        {
            Vector3 newFillerPosition = originalRoomFillerPosition;

            newFillerPosition = new Vector3(newFillerPosition.x * positiveMultiplier, newFillerPosition.y, -newFillerPosition.z * negativeMultiplier);
            transform.FindChild("RoomFiller").localPosition = newFillerPosition;
        }
    }

    public void ResetLocation()
    {
        SetLocation(previousConnectedPosition, previousConnectedRotation);
    }

    public void SetLocation(Vector3 gridCoordinates, Quaternion rotation)
    {
        ClearOldBlocks();
        transform.position = CalculateGridToReal(gridCoordinates);
        gridLocation = gridCoordinates;
        transform.rotation = rotation;
        float updateRotationY = transform.eulerAngles.y;
        if (updateRotationY == -90) { updateRotationY = 270; }
        if (updateRotationY == 360) { updateRotationY = 0;  }
        UpdateCoordinates(updateRotationY);
        
        UpdateNewBlocks();
    }

    public void MoveDirection( Vector3 direction)
    {
        //Move when off cooldown, so it moves one block every second.
        if(moveCooldownTimer <= 0)
        {
            //Remove from the list.
            ClearOldBlocks();
            //Check if we can move the origin in that direction.
            if(CheckAvailableOrigin(gridLocation + direction, gridLocation) == gridLocation + direction)
            {
                gridLocation += direction;
                gridLocation = new Vector3(Mathf.Clamp(gridLocation.x, gridInfo.gridMin, gridInfo.gridMax), 0, Mathf.Clamp(gridLocation.z, gridInfo.gridMin, gridInfo.gridMax));
                //Debug.Log(gridLocation);
                //Check if available with updated gridlocation.
                if (CheckFullAvailability(coordinatesOccupied))
                {
                    transform.position = CalculateGridToReal(gridLocation);
                    //Debug.Log(transform.position);
                    //Reset cooldown since we actually moved.
                    moveCooldownTimer = moveTick;   
                    
                }
                else //Not available. Revert to old grid location.
                {
                    gridLocation -= direction;
                }     
            }
            UpdateNewBlocks();
        }
    }

    public void UpdateCoordinates(float rotation)
    {
        List<Vector3> newList = new List<Vector3>();
        float changeInRotation = rotation - rotationY;
        if (changeInRotation == 0) { return; }
        if (changeInRotation == -270) { changeInRotation = 90; }
        if (changeInRotation == -180) { changeInRotation = 180; }
        foreach (Vector3 coordinate in coordinatesOccupied)
        {
      
            //Debug.Log(changeInRotation);
            Vector3 newCoordinate = RotatePositionByOrigin(coordinate, changeInRotation);
            //Debug.Log(coordinate + " "  + newCoordinate);
            newList.Add(newCoordinate);
        }

        float lowestX = 0;
        float lowestZ = 0;
        //Find the bottom left coordinate.
        foreach (Vector3 coordinate in newList)
        {
            if (coordinate.x < lowestX)
            {
                lowestX = coordinate.x;
            }
            if (coordinate.z < lowestZ)
            {
                lowestZ = coordinate.z;
            }
        }
        Vector3 updateVector = new Vector3((lowestX), 0, (lowestZ));

        //With the update vector add to every vector to move the origin to the bottom left again.
        List<Vector3> tempList = new List<Vector3>();
        foreach (Vector3 coordinate in newList)
        {
            Vector3 tempCoordinate = coordinate - updateVector;
            if (tempCoordinate.x < 0 || tempCoordinate.y < 0 || tempCoordinate.z < 0)
            {
                //Debug.Log("ERROR!!! Shouldn't allow negative coordinate values!");
            }
            tempList.Add(tempCoordinate);
        }
        
        if (CheckFullAvailability(tempList))
        {
            //Debug.Log("Rotate!");
            coordinatesOccupied = tempList;
            transform.localEulerAngles = new Vector3(0, rotation, 0);
            rotationY = rotation;
            if (rotationY == 360)
            {
                rotationY = 0;
            }
            if (rotationY == -90)
            {
                rotationY = 270;
            }
            //Debug.Log(rotationY);
            foreach (Transform myDoor in transform.GetChild(0))
            {
                if (myDoor.tag == "Door")
                {
                    //Debug.Log("Resetting: " + myDoor);
                    myDoor.GetComponent<DoorScript>().ResetDoors();
                }
            }
        }
        MoveOrigin(rotation);
   
    }

    public void UpdateNewBlocks()
    {
        foreach(Vector3 temp in coordinatesOccupied)
        {
            //Debug.Log("Adding: " + (temp + gridLocation));
            gridInfo.AddBlock(temp + gridLocation, transform);
        }
    }

    //Remove all of our blocks from the gridInfo.
    public void ClearOldBlocks()
    {
        foreach(Vector3 temp in coordinatesOccupied)
        {
            //Debug.Log("Removing: " + (temp + gridLocation) + "" + transform);
            gridInfo.RemoveBlock(temp + gridLocation, transform);
        }
    }
    
    //Readjust coordinates to correspond with rotation, either 90 or -90.
    public Vector3 RotatePositionByOrigin(Vector3 coordinate, float rotation)
    {
        float posX = coordinate.x;
        float posY = coordinate.y;
        float posZ = coordinate.z;
        //Debug.Log(rotation);
        Vector3 newCoordinate = new Vector3(0, 0, 0);
        if (rotation == 90)//Rotate clockwise
        {
            newCoordinate = new Vector3(posZ, posY, -posX);
        }
        else if(rotation == -90)//Rotate counter-clockwise
        {
            newCoordinate = new Vector3(-posZ, posY, posX);
        }
        else if(rotation == 180)
        {
            newCoordinate = new Vector3(-posZ, posY, -posX);
        }
        return newCoordinate;
    }

    //Check that all offshoot blocks won't be placed into a used block.
    public bool CheckFullAvailability(List<Vector3> allCoordinates)
    {
        foreach(RoomInfo roomInfo in gridInfo.GetComponent<GridInfo>().usedGridBlocks)
        {
            foreach(Vector3 takenBlock in allCoordinates)
            {
                Vector3 coordinate = new Vector3(roomInfo.coordinate.x, roomInfo.coordinate.y, roomInfo.coordinate.z);

                if (coordinate.Equals(takenBlock + gridLocation))
                {
                    Debug.Log("Offshoot block: " + (takenBlock + CalculateRealToGrid(transform.position)) + " can't be moved here: " + coordinate);
                    return false;
                }
            }
        }
        //Debug.Log("All blocks check out.");
        return true;
    }

    //First check if the origin point for this room is attempting to be moved to an unavailable position.
    public Vector3 CheckAvailableOrigin (Vector3 newCoordinates, Vector3 oldCoordinates)
    {
        foreach (RoomInfo room in gridInfo.GetComponent<GridInfo>().usedGridBlocks)
        {
            if (room.coordinate.Equals(newCoordinates))
            {
                //Check if origin is a taken block or simply a pivot.
                foreach(Vector3 takenBlock in coordinatesOccupied)
                {
                    //If coordinate in list is the same as our new origin, the origin is an occupied block.
                    if(takenBlock + CalculateRealToGrid(transform.position) == newCoordinates)
                    {
                        //Debug.Log("Spot is taken! Block: " + takenBlock + " in coordinate: "+ newCoordinates);
                        //targetGrid = new Vector3(0, 0, 0);
                        return oldCoordinates;
                    }
                }
              
            }
        }
        //Debug.Log("Spot available! Origin: " + newCoordinates);
        return newCoordinates;
    }

    public Vector3 CalculateGridToReal(Vector3 coordinates)
    {
        Vector3 location;
        gridInfo = gridBase.GetComponent<GridInfo>();
        coordinates = new Vector3(Mathf.Clamp(coordinates.x, gridInfo.gridMin, gridInfo.gridMax),
                                    coordinates.y,
                                    Mathf.Clamp(coordinates.z, gridInfo.gridMin, gridInfo.gridMax));
        location = new Vector3(coordinates.x * blockLength, height, coordinates.z * blockLength);
        return location;
    }

    public Vector3 CalculateRealToGrid(Vector3 position)
    {
        Vector3 coordinates;
        gridInfo = gridBase.GetComponent<GridInfo>();
        float remainderX = position.x % blockLength;
        float remainderY = position.y % blockLength;
        gridXPosition = Mathf.Clamp((int)(position.x / blockLength), gridInfo.gridMin, gridInfo.gridMax);
        gridYPosition = Mathf.Clamp((int)(position.z / blockLength), gridInfo.gridMin, gridInfo.gridMax);
        if (remainderX >= blockLength/2)
        {
            gridXPosition++;
        }
        if(remainderY >= blockLength/2)
        {
            gridYPosition++;
        }
        coordinates = new Vector3(gridXPosition, height, gridYPosition); 

        return coordinates;
    }
    
    public void InitToGridInfo()
    {
        foreach (Vector3 takenBlock in coordinatesOccupied)
        {
            gridInfo.AddBlock(takenBlock + gridLocation, transform);
        }
    }
}
