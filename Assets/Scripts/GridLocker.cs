using UnityEngine;
using UnityEditor;
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

    public enum Rotation{ Neutral, CW, Flipped, CCW };
    public Rotation currentRotation;


    public List<Transform> doors = new List<Transform>();
    public bool amIConnected;

    Transform gridBase, doorTracker;

    [SerializeField]
    public float height;

    [SerializeField]
    public Vector3 gridLocation;

    [SerializeField]
    float rotation;

    public float copyRotationDifference = 0;

    [SerializeField]
    public bool locked = true;

    [SerializeField]
    public int roomNumber;
        
    [SerializeField]
    public List<CoordinateInfo> coordinatesOccupied;

    private float gridXPosition, gridYPosition;
    private Vector3 currentLocation, adjustmentVector, originalRoomFillerPosition, currentRoomFillerPosition;
    private GridInfo gridInfo;
    private float blockLength;
    public float moveTick;
    private float moveCooldownTimer = 0;
    [HideInInspector]
    public float rotationY;
    public bool childLocked = false, moving = false;
    public List<Transform> connectedDoors;
    Vector2 dimensions;
    int counter;
    public int doorCount = 0;
    [HideInInspector]
    public Transform copyOf;

    // Use this for initialization
    void Start () {

        gridBase = GameObject.Find("GridBase").transform;
        doorTracker = GameObject.Find("GameManager").transform;

        currentLocation = CalculateGridToReal(gridLocation);
        if (transform.name.Contains("Room"))
        {
            originalRoomFillerPosition = transform.FindChild("RoomFiller").localPosition;
        }
        else
        {
            originalRoomFillerPosition = transform.position;
        }
       

       
        gridInfo = gridBase.GetComponent<GridInfo>();
        blockLength = gridInfo.blockLength;
        Physics.IgnoreLayerCollision(12, 14, true);
        moveTick = gridInfo.moveTick;

        transform.position = CalculateGridToReal(gridLocation);
        rotationY = transform.eulerAngles.y;
        //UpdateCoordinates(rotationY);
        MoveOrigin();

        if(gridInfo.usedGridBlocks == null)
        {
            gridInfo.InitList();
        }
        
        InitToGridInfo();
        if (isRoom())
        {
            //InitDoors();
        }

        dimensions = GetDimensions();

    }

    // Update is called once per framsdses
    void Update()
    {
        gridBase = GameObject.Find("GridBase").transform;
        gridInfo = gridBase.GetComponent<GridInfo>();
        if (moveCooldownTimer <= moveTick && moveCooldownTimer >= 0)
        {
            moveCooldownTimer -= Time.deltaTime;
        }

        /*
        //0 Degrees
        if (isCloseTo(rotation, 0) || isCloseTo(rotation, 360))
        {
            rotation = 0;
        }
        //90 Degrees
        else if (isCloseTo(rotation, 90) || isCloseTo(rotation, -270))
        {
            rotation = 90;
        }
        //180 Degrees
        else if (isCloseTo(rotation, 180) || isCloseTo(rotation, -180))
        {
            rotation = 180;
        }
        //270 Degrees
        else if (isCloseTo(rotation, 270) || isCloseTo(rotation, -90))
        {
            rotation = 270;
        }*/
    }

    public bool isCloseTo(float value, float targetRotation)
    {
        if(targetRotation - 45 < value && targetRotation + 45 > value)
        {
            return true;
        }
        return false;
    }

    public bool CheckIfConnected()
    {
        foreach (Transform door in doors)
        {
            if (door.GetComponent<DoorScript>().isConnected)
            {
                return true;
            }
        }
        return false;
    }

    //Gives us the dimensions of the whole cubic block this piece theoretically occupies.
    public Vector2 GetDimensions()
    {
        Vector2 dimensions = new Vector2(0, 0);
        foreach(CoordinateInfo coordInfo in coordinatesOccupied)
        {
            Vector3 coordinate = coordInfo.coordinateOccupied;
            if(coordinate.x > dimensions.x)
            {
                dimensions.x = coordinate.x;
            }
            if(coordinate.z > dimensions.y)
            {
                dimensions.y = coordinate.z;
            }
        }
        //Correct for 0 being a value in coordinates.
        dimensions = dimensions + new Vector2(1, 1);
        return dimensions;

    }

    //Move origin with the room.
    public void MoveOrigin()
    {
        if (isRoom())
        {

            //Move pivot on rotation.   
            float positiveMultiplier, negativeMultiplier;
            //Positive should be smaller.
            positiveMultiplier = 0.999f;//.0009995f;
            //Negative should be more negative;
            negativeMultiplier = 0.999f;//0.9990005f;
                                        //Debug.Log(rotationY);
                                        //Debug.Log("Rotation: " + rotationY);

            Rotation comparisonRotation = currentRotation;

            /*
            if (copyRotationDifference == 90)
            {
                comparisonRotation = returnRotateCW(comparisonRotation);
            }
            else if (copyRotationDifference == 180)
            {
                comparisonRotation = returnRotateCW(comparisonRotation);
                comparisonRotation = returnRotateCW(comparisonRotation);
            }
            else if (copyRotationDifference == 270 || copyRotationDifference == -90)
            {
                comparisonRotation = returnRotateCCW(comparisonRotation);
            }*/
           
            if (comparisonRotation.Equals(Rotation.Neutral))
            {
                //Debug.Log("detected rotation");
                Vector3 newFillerPosition = originalRoomFillerPosition;
                newFillerPosition = new Vector3(newFillerPosition.x * negativeMultiplier, newFillerPosition.y, newFillerPosition.z * positiveMultiplier);
                transform.FindChild("RoomFiller").localPosition = newFillerPosition;
            }
            else if (comparisonRotation.Equals(Rotation.CW))
            {
                Vector3 newFillerPosition = originalRoomFillerPosition;
                newFillerPosition = new Vector3(-newFillerPosition.x * negativeMultiplier, newFillerPosition.y, newFillerPosition.z * negativeMultiplier);
                transform.FindChild("RoomFiller").localPosition = newFillerPosition;
            }
            else if (comparisonRotation.Equals(Rotation.Flipped))
            {
                Vector3 newFillerPosition = originalRoomFillerPosition;

                newFillerPosition = new Vector3(-newFillerPosition.x * positiveMultiplier, newFillerPosition.y, -newFillerPosition.z * negativeMultiplier);
                transform.FindChild("RoomFiller").localPosition = newFillerPosition;
            }
            else if (comparisonRotation.Equals(Rotation.CCW))
            {
                Debug.Log("No rotation.");
                Vector3 newFillerPosition = originalRoomFillerPosition;
                //Debug.Log("Original: " + originalRoomFillerPosition);
                newFillerPosition = new Vector3(newFillerPosition.x * positiveMultiplier, newFillerPosition.y, -newFillerPosition.z * positiveMultiplier);
                transform.FindChild("RoomFiller").localPosition = newFillerPosition;
            }

        }
        else
        {

            //Move pivot on rotation.   
            float positiveMultiplier, negativeMultiplier;
            //Positive should be smaller.
            positiveMultiplier = 0.999f;//.0009995f;
            //Negative should be more negative;
            negativeMultiplier = 0.999f;//0.9990005f;
                                        //Debug.Log(rotationY);
                                        //Debug.Log("Rotation: " + rotationY);
                                        //Vector2 dimensions = GetDimensions();
                                        //Debug.Log(dimensions);
                                        //Debug.Log("PLACEHOLDER");
            Rotation comparisonRotation = currentRotation;
            if (copyRotationDifference == 90)
             {
                 comparisonRotation = returnRotateCCW(comparisonRotation);
             }
            else if (copyRotationDifference == 180)
            {
              comparisonRotation = returnRotateCW(comparisonRotation);
              comparisonRotation = returnRotateCW(comparisonRotation);
            }
             else if (copyRotationDifference == 270 || copyRotationDifference == -90)
            {
              comparisonRotation = returnRotateCW(comparisonRotation);
            }

            if (comparisonRotation.Equals(Rotation.Neutral))
            {
                transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
            }
            else if (comparisonRotation.Equals(Rotation.CW))
            {
                transform.GetChild(0).localPosition = new Vector3(-dimensions.x * gridInfo.blockLength, 0, 0);
            }
            else if (comparisonRotation.Equals(Rotation.Flipped))
            {
                transform.GetChild(0).localPosition = new Vector3(-dimensions.x * gridInfo.blockLength, 0, -dimensions.y * gridInfo.blockLength);
            }
            else if (comparisonRotation.Equals(Rotation.CCW))
            {
                transform.GetChild(0).localPosition = new Vector3(0, 0, -dimensions.y * gridInfo.blockLength);
            }
        }
    }

    //Set the location of the room to this location and rotation
    public void SetLocation(Transform targetRoom)
    {
        ClearOldBlocks();
        GridLocker targetScript = targetRoom.GetComponent<GridLocker>();
        transform.position = targetRoom.position;
        transform.rotation = Quaternion.Euler(new Vector3(targetRoom.eulerAngles.x, targetRoom.eulerAngles.y + targetScript.copyRotationDifference, targetRoom.eulerAngles.z));
       

        float rotationinput = targetRoom.eulerAngles.y + targetScript.copyRotationDifference;
        if(rotationinput > 360)
        {
            rotationinput -= 360;
        }
        Debug.Log("Rotation Input: " + rotationinput);
        currentRotation = targetScript.currentRotation;
        MoveOrigin();

        gridLocation = targetScript.gridLocation;
        //RemoveAllDoors();

        //rotation = targetScript.rotation;
        //Debug.Log(updateRotationY);

        for (int i = 0; i < coordinatesOccupied.Count; i++)
        {
            coordinatesOccupied[i] = targetScript.coordinatesOccupied[i];
        }

       
       
       
        //RemoveAllDoors();
        //InitDoors();
        UpdateNewBlocks();
    }

    //Move the room without checks
    public void MoveDirectionNoCheck( Vector3 direction)
    {
        if (moveCooldownTimer <= 0)
        {
            ClearOldBlocks();
            //Check if we can move the origin in that direction.
            gridLocation += direction;
            gridLocation = new Vector3(Mathf.Clamp(gridLocation.x, gridInfo.gridMin, gridInfo.gridMax), 0, Mathf.Clamp(gridLocation.z, gridInfo.gridMin, gridInfo.gridMax));

            

            //Debug.Log(gridLocation);
            //Check if available with updated gridlocation.
            transform.position = CalculateGridToReal(gridLocation);
            //Debug.Log(transform.position);
            //Reset cooldown since we actually moved.
            ResetAllDoors();
            moveCooldownTimer = moveTick;

            /*
            foreach (CoordinateInfo coordInfo in coordinatesOccupied)
            {
                Vector3 updatedCoordinates = coordInfo.coordinateOccupied + direction + gridLocation;
                gridBase.GetComponent<GridInfo>().RemoveBlock(coordInfo.coordinateOccupied, transform);
                gridBase.GetComponent<GridInfo>().AddBlock(updatedCoordinates, transform);
            }*/


            ConnectAllDoors();
           
            UpdateNewBlocks();
            amIConnected = CheckIfConnected();
        }
    }

    public void ResetAllDoors()
    {
        foreach(Transform door in doors)
        {
            door.GetComponent<DoorScript>().ResetDoors();
        }
    }

    //Move the room with checks (Deappreciated since we no longer move rooms directly)
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

    public void RotateCW()
    {
        if(currentRotation == Rotation.Neutral)
        {
            currentRotation = Rotation.CW;
        }
        else if(currentRotation == Rotation.CW)
        {
            currentRotation = Rotation.Flipped;
        }
        else if(currentRotation == Rotation.Flipped)
        {
            currentRotation = Rotation.CCW;
        }
        else if(currentRotation == Rotation.CCW)
        {
            currentRotation = Rotation.Neutral;
        }
        MoveOrigin();
    }

    public Rotation returnRotateCW(Rotation current)
    {
        if (current== Rotation.Neutral)
        {
            return Rotation.CW;
        }
        else if (current == Rotation.CW)
        {
           return Rotation.Flipped;
        }
        else if (current == Rotation.Flipped)
        {
            return Rotation.CCW;
        }
        else if (current== Rotation.CCW)
        {
            return Rotation.Neutral;
        }
        return Rotation.Neutral;
    }

    public Rotation returnRotateCCW(Rotation current)
    {
        if (current == Rotation.Neutral)
        {
            return Rotation.CCW;
        }
        else if (current == Rotation.CW)
        {
            return Rotation.Neutral;
        }
        else if (current == Rotation.Flipped)
        {
            return Rotation.CW;
        }
        else if (current == Rotation.CCW)
        {
            return Rotation.Flipped;
        }
        return Rotation.Neutral;
    }

    public void RotateCCW()
    {
        if (currentRotation == Rotation.Neutral)
        {
            currentRotation = Rotation.CCW;
        }
        else if (currentRotation == Rotation.CW)
        {
            currentRotation = Rotation.Neutral;
        }
        else if (currentRotation == Rotation.Flipped)
        {
            currentRotation = Rotation.CW;
        }
        else if (currentRotation == Rotation.CCW)
        {
            currentRotation = Rotation.Flipped;
        }
        MoveOrigin();
    }

    //Check if it is a room
    public bool isRoom()
    {
        if(transform.childCount > 0)
        {
            if (transform.GetChild(0).name == "RoomFiller")
            {
                return true;
            }     
        }
        return false;
    }

    public DoorLocation RotateDoorBools(DoorLocation doorBool, float rotationChange, Vector3 newCoordinate)
    {
        DoorLocation newDoors = new DoorLocation();
        if(doorBool == null)
        {
            return null;
        }
        if (rotationChange == 90)
        {
            if (doorBool.north)
            {
                newDoors.east = true;
                //newDoors.eastDoor = doorBool.northDoor;
                //newDoors.eastDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.eastDoor.GetComponent<DoorScript>().location = DoorScript.Direction.East;
            }
            if (doorBool.east)
            {
                newDoors.south = true;
                //newDoors.southDoor = doorBool.eastDoor;
                //newDoors.southDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.southDoor.GetComponent<DoorScript>().location = DoorScript.Direction.South;
            }
            if (doorBool.south)
            {
                newDoors.west = true;
                //newDoors.westDoor = doorBool.southDoor;
                //newDoors.westDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.westDoor.GetComponent<DoorScript>().location = DoorScript.Direction.West;
            }
            if (doorBool.west)
            {
                newDoors.north = true;
                //newDoors.northDoor = doorBool.westDoor;
                //newDoors.northDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.northDoor.GetComponent<DoorScript>().location = DoorScript.Direction.North;
            }
        }
        else if (rotationChange == -90 || rotationChange == 270)
        {
            if (doorBool.north)
            {
                newDoors.west = true;
                //newDoors.westDoor = doorBool.northDoor;
                //newDoors.westDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.westDoor.GetComponent<DoorScript>().location = DoorScript.Direction.West;
            }
            if (doorBool.west)
            {   
                newDoors.south = true;
                //newDoors.southDoor = doorBool.westDoor;
                //newDoors.southDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.southDoor.GetComponent<DoorScript>().location = DoorScript.Direction.South;
            }
            if (doorBool.south)
            {
                newDoors.east = true;
                //newDoors.eastDoor = doorBool.southDoor;
                //newDoors.eastDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.eastDoor.GetComponent<DoorScript>().location = DoorScript.Direction.East;
            }
            if (doorBool.east)
            {
                newDoors.north = true;
                //newDoors.northDoor = doorBool.eastDoor;
                //newDoors.northDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.northDoor.GetComponent<DoorScript>().location = DoorScript.Direction.North;
            }
        }
        else if (rotationChange == 180)
        {
            if (doorBool.north)
            {
                newDoors.south = true;
                //newDoors.southDoor = doorBool.northDoor;
                //newDoors.southDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.southDoor.GetComponent<DoorScript>().location = DoorScript.Direction.South;
            }
            if (doorBool.south)
            {
                newDoors.north = true;
                //newDoors.northDoor = doorBool.southDoor;
                //newDoors.northDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.northDoor.GetComponent<DoorScript>().location = DoorScript.Direction.North;
            }
            if (doorBool.west)
            {
                newDoors.east = true;
                //newDoors.eastDoor = doorBool.westDoor;
                //newDoors.eastDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.eastDoor.GetComponent<DoorScript>().location = DoorScript.Direction.East;
            }
            if (doorBool.east)
            {
                newDoors.west = true;
                //newDoors.westDoor = doorBool.eastDoor;
                //newDoors.westDoor.GetComponent<DoorScript>().coordinateOfDoor = newCoordinate;
                //newDoors.westDoor.GetComponent<DoorScript>().location = DoorScript.Direction.West;        
            }
        }
        return newDoors;
    }

    public Vector3 GetUpdateVector()
    {
        float lowestX = 0;
        float lowestZ = 0;
        foreach (CoordinateInfo coordInfo in coordinatesOccupied)
        {       
            if (coordInfo.coordinateOccupied.x < lowestX)
            {
                lowestX = coordInfo.coordinateOccupied.x;
            }
            if (coordInfo.coordinateOccupied.z < lowestZ)
            {
                lowestZ = coordInfo.coordinateOccupied.z;
            }
        }
        Vector3 updateVector = new Vector3((lowestX), 0, (lowestZ));
        Debug.Log(updateVector);
        return updateVector;
    }

    public void UpdateCoordinates(float rotation)
    {
        List<CoordinateInfo> newList = new List<CoordinateInfo>();
        float changeInRotation = rotation - rotationY;
        Debug.Log("ALERT: " + changeInRotation);
        if (changeInRotation == 0) { return; }
        if (changeInRotation == -270) { changeInRotation = 90; }
        if (changeInRotation == -180) { changeInRotation = 180; }

        foreach (CoordinateInfo coordInfo in coordinatesOccupied)
        {
            //Debug.Log(changeInRotation);
            Vector3 newCoordinate = RotatePositionByOrigin(coordInfo.coordinateOccupied, changeInRotation);
            DoorLocation newDoors = RotateDoorBools(coordInfo.doors, changeInRotation, newCoordinate);
            CoordinateInfo newCoordInfo = new CoordinateInfo();
            newCoordInfo.coordinateOccupied = newCoordinate;
            newCoordInfo.doors = newDoors;
            Debug.Log(newCoordInfo.coordinateOccupied);
            newList.Add(newCoordInfo);
        }
        float lowestX = 0;
        float lowestZ = 0;
        //Find the bottom left coordinate.
        foreach (CoordinateInfo coordInfo in newList)
        {
            if (coordInfo.coordinateOccupied.x < lowestX)
            {
                lowestX = coordInfo.coordinateOccupied.x;
            }
            if (coordInfo.coordinateOccupied.z < lowestZ)
            {
                lowestZ = coordInfo.coordinateOccupied.z;
            }
        }
        Vector3 updateVector = new Vector3((lowestX), 0, (lowestZ));

        //With the update vector add to every vector to move the origin to the bottom left again.
        List<CoordinateInfo> tempList = new List<CoordinateInfo>();
        foreach (CoordinateInfo coordInfo in newList)
        {
            Vector3 tempCoordinate = coordInfo.coordinateOccupied - updateVector;
            CoordinateInfo newCoordInfo = new CoordinateInfo();
            newCoordInfo.coordinateOccupied = tempCoordinate;
            //newCoordInfo.doors = UpdateAllDoorCoordinates(coordInfo.doors, updateVector);
            //Debug.Log(updateVector + " " + newCoordInfo.doors);
            tempList.Add(newCoordInfo);
        }
        /*
        if (CheckFullAvailability(tempList) || !isRoom())
        {*/
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
        /*}*/
        Debug.Log("Rotation: " + rotation);
        MoveOrigin();
        //ResetAllDoors();
        //ConnectAllDoors();

    }

    public DoorLocation UpdateAllDoorCoordinates(DoorLocation doors, Vector3 updateVector)
    {
        DoorLocation newDoors = doors;
        if (doors.north)
        {
            newDoors.northDoor.GetComponent<DoorScript>().coordinateOfDoor -= updateVector;
        }
        if (doors.south)
        {
            newDoors.southDoor.GetComponent<DoorScript>().coordinateOfDoor -= updateVector;
        }
        if (doors.west)
        {
            newDoors.westDoor.GetComponent<DoorScript>().coordinateOfDoor -= updateVector;
        }
        if (doors.east)
        {
            newDoors.eastDoor.GetComponent<DoorScript>().coordinateOfDoor -= updateVector;
        }
        return newDoors;
    }

    public void UpdateNewBlocks()
    {
        foreach(CoordinateInfo temp in coordinatesOccupied)
        {
            //Debug.Log("Adding: " + (temp + gridLocation));
            gridInfo.AddBlock(temp.coordinateOccupied + gridLocation, transform);
        }
    }

    //Remove all of our blocks from the gridInfo.
    public void ClearOldBlocks()
    {
        foreach(CoordinateInfo temp in coordinatesOccupied)
        {
            //Debug.Log("Removing: " + (temp + gridLocation) + "" + transform);
            gridInfo.RemoveBlock(temp.coordinateOccupied + gridLocation, transform);
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
            newCoordinate = new Vector3(posZ, posY, posX);
        }
        else
        {
            newCoordinate = new Vector3(posZ, posY, posX);
        }
        
        return newCoordinate;
    }

    //Check that all offshoot blocks won't be placed into a used block.
    public bool CheckFullAvailability(List<CoordinateInfo> allCoordinates)
    {
        foreach(RoomInfo roomInfo in gridInfo.GetComponent<GridInfo>().usedGridBlocks)
        {
            foreach(CoordinateInfo coordInfo in allCoordinates)
            {
                Vector3 coordinate = new Vector3(roomInfo.coordinate.x, roomInfo.coordinate.y, roomInfo.coordinate.z);

                if (coordinate.Equals(coordInfo.coordinateOccupied + gridLocation))
                {
                    //Debug.Log("Offshoot block: " + (takenBlock + CalculateRealToGrid(transform.position)) + " can't be moved here: " + coordinate);
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
                foreach(CoordinateInfo coordInfo in coordinatesOccupied)
                {
                    //If coordinate in list is the same as our new origin, the origin is an occupied block.
                    if(coordInfo.coordinateOccupied + CalculateRealToGrid(transform.position) == newCoordinates)
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
        Transform gridBase = GameObject.Find("GridBase").transform;
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
        if (isRoom())
        {
            foreach (CoordinateInfo coordInfo in coordinatesOccupied)
            {
                gridInfo.AddBlock(coordInfo.coordinateOccupied + gridLocation, transform);
            }
        }
    }

    public void InitDoors()
    {
        if (isRoom())
        {
            foreach (CoordinateInfo coordInfo in coordinatesOccupied)
            {
                Vector3 currentLocation = CalculateGridToReal(coordInfo.coordinateOccupied + gridLocation);
                if (coordInfo.doors.north)
                {
                    coordInfo.doors.northDoor = PlaceDoor(currentLocation + new Vector3(gridInfo.blockLength / 2, 0, gridInfo.blockLength), new Vector3(0, 0, 0), DoorScript.Direction.North, coordInfo.coordinateOccupied);
                }
                if (coordInfo.doors.east)
                {
                    coordInfo.doors.eastDoor = PlaceDoor(currentLocation + new Vector3(gridInfo.blockLength, 0, gridInfo.blockLength / 2), new Vector3(0, 90, 0), DoorScript.Direction.East, coordInfo.coordinateOccupied);
                }
                if (coordInfo.doors.south)
                {
                    coordInfo.doors.southDoor = PlaceDoor(currentLocation + new Vector3(gridInfo.blockLength / 2, 0, 0), new Vector3(0, 0, 0), DoorScript.Direction.South, coordInfo.coordinateOccupied);
                }
                if (coordInfo.doors.west)
                {
                    coordInfo.doors.westDoor = PlaceDoor(currentLocation + new Vector3(0, 0, gridInfo.blockLength / 2), new Vector3(0, 90, 0), DoorScript.Direction.West, coordInfo.coordinateOccupied);
                }
            }
        }
    }

    public void RemoveAllDoors()
    {
        foreach(Transform door in doors)
        {
            Destroy(door.gameObject);
        }
        doors.Clear();
        doorCount = 0;

        foreach(CoordinateInfo coordInfo in coordinatesOccupied)
        {
            coordInfo.doors.northDoor = null;
            coordInfo.doors.southDoor = null;
            coordInfo.doors.eastDoor = null;
            coordInfo.doors.westDoor = null;
        }
    }
    
    public void InitDoorUIOnly()
    {
        if (!isRoom())
        {
            foreach (CoordinateInfo coordInfo in coordinatesOccupied)
            {
               
                Vector3 currentLocation = CalculateGridToReal(coordInfo.coordinateOccupied + gridLocation);
                if (coordInfo.doors.north)
                {
                    coordInfo.doors.northDoor = PlaceDoor(currentLocation + new Vector3(gridInfo.blockLength / 2, 0, gridInfo.blockLength), new Vector3(0, 0, 0), DoorScript.Direction.North, coordInfo.coordinateOccupied);
                    coordInfo.doors.northDoor.GetChild(0).GetComponent<Renderer>().enabled = false;
                }
                if (coordInfo.doors.east)
                {
                    coordInfo.doors.eastDoor = PlaceDoor(currentLocation + new Vector3(gridInfo.blockLength, 0, gridInfo.blockLength / 2), new Vector3(0, 90, 0), DoorScript.Direction.East, coordInfo.coordinateOccupied);
                    coordInfo.doors.eastDoor.GetChild(0).GetComponent<Renderer>().enabled = false;
                }
                if (coordInfo.doors.south)
                {
                    coordInfo.doors.southDoor = PlaceDoor(currentLocation + new Vector3(gridInfo.blockLength / 2, 0, 0), new Vector3(0, 0, 0), DoorScript.Direction.South, coordInfo.coordinateOccupied);
                    coordInfo.doors.southDoor.GetChild(0).GetComponent<Renderer>().enabled = false;
                }
                if (coordInfo.doors.west)
                {
                    coordInfo.doors.westDoor = PlaceDoor(currentLocation + new Vector3(0, 0, gridInfo.blockLength / 2), new Vector3(0, 90, 0), DoorScript.Direction.West, coordInfo.coordinateOccupied);
                    coordInfo.doors.westDoor.GetChild(0).GetComponent<Renderer>().enabled = false;
                }
                //Instantiate(Resources.Load("doubleDoor"), )
            }
            //Debug.Log(transform.name + "'s doors: " + doorCount);
        }
    }
    
    public Transform PlaceDoor(Vector3 position, Vector3 rotation, DoorScript.Direction direction, Vector3 relativeCoordinate)
    {
        GameObject temp = Instantiate(Resources.Load("DoubleDoor")) as GameObject;
        doors.Add(temp.transform);
        temp.transform.position = position;
        temp.transform.rotation = Quaternion.Euler(rotation);
        temp.GetComponent<DoorScript>().room = transform;
        temp.GetComponent<DoorScript>().location = direction;
        temp.GetComponent<DoorScript>().coordinateOfDoor = relativeCoordinate;
        doorCount++;
        return temp.transform;
    }

    //Create a white overlay invisible to the kid that parents to the transform placeholder.
    public Transform CreateInvisibleOverlay(Transform placeholder)
    {
        GameObject overheadParent = new GameObject("Placeholder Collection");
        GameObject roomRotator = new GameObject("RoomRotator");
        roomRotator.transform.parent = overheadParent.transform;
        overheadParent.tag = "Room";
        overheadParent.AddComponent<GridLocker>();
        overheadParent.GetComponent<GridLocker>().gridLocation = coordinatesOccupied[0].coordinateOccupied;
        overheadParent.GetComponent<GridLocker>().coordinatesOccupied = coordinatesOccupied;
        overheadParent.GetComponent<GridLocker>().copyOf = transform;
        overheadParent.GetComponent<GridLocker>().currentRotation = currentRotation;
        //overheadParent.GetComponent<GridLocker>().InitDoorUIOnly();
        overheadParent.GetComponent<GridLocker>().copyRotationDifference = transform.eulerAngles.y;
        foreach (CoordinateInfo coordInfo in coordinatesOccupied)
        {
            //Vector3 realCoordinate = coordinate + gridLocation;
            Vector3 realPosition = CalculateGridToReal(coordInfo.coordinateOccupied);
            realPosition.y = 10.5f;
            Transform temp = Instantiate(placeholder, realPosition, Quaternion.identity) as Transform;
            temp.parent = roomRotator.transform;
           
            //Instantiate(placeholder, )
        }
        
        
        return overheadParent.transform;
    }

    public void ConnectAllDoors()
    {
        foreach(Transform door in doors)
        {
            door.GetComponent<DoorScript>().ConnectDoors();
        }
    }

    public bool isViableLocation()
    {
        foreach(CoordinateInfo coordInfo in coordinatesOccupied)
        {
            foreach(RoomInfo roomInfo in gridBase.GetComponent<GridInfo>().usedGridBlocks)
            {
                if(roomInfo.coordinate == (coordInfo.coordinateOccupied + gridLocation) && roomInfo.room != transform)
                {
                    return false;
                }
            }
        }
        return true;
    }

}

//Custom class to store coordinate information
[System.Serializable]
public class CoordinateInfo
{
    public DoorLocation doors;
    public Vector3 coordinateOccupied;
}

//Custom class to store doorlocations on this block
[System.Serializable]
public class DoorLocation
{
    public bool north= false, east = false, south = false, west = false;
    public Transform northDoor, eastDoor, southDoor, westDoor;
}


//CoordinateInfo Drawer for Inspector
[CustomPropertyDrawer(typeof(CoordinateInfo))]
public class CoordinateInfoDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        label.text = "Coordinate Info";
        position.height = 10;
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Get rid of indent among child labels
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
       
        // Calculate the rectangles
        Rect doorRect = new Rect(position.x, position.y, 200, position.height);
        Rect coordinateRect = new Rect(position.x, position.y + 20, 200, position.height);

        //Change the height of the property
        

        // Draw the fields
        EditorGUI.PropertyField(doorRect, property.FindPropertyRelative("doors"), GUIContent.none);
        EditorGUI.PropertyField(coordinateRect, property.FindPropertyRelative("coordinateOccupied"), GUIContent.none);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float extraHeight = 20.0f;
        return base.GetPropertyHeight(property, label) + extraHeight;
    }
}

//DoorLocation Drawer for Inspector
[CustomPropertyDrawer (typeof(DoorLocation))]
public class DoorLocationDrawer : PropertyDrawer
{
    //Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        //Get rid of indent among child labels
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate the rectangles
        Rect doorNorthRect = new Rect(position.x, position.y, 200, position.height);

        //GUI Labels
        GUIContent northLabel = new GUIContent(); northLabel.text = "N";
        GUIContent eastLabel = new GUIContent(); eastLabel.text = "E";
        GUIContent southLabel = new GUIContent(); southLabel.text = "S";
        GUIContent westLabel = new GUIContent(); westLabel.text = "W";

        GUIContent[] labels = { northLabel, eastLabel, southLabel, westLabel };

        //Draw the fields
        //EditorGUI.PropertyField(doorNorthRect, property.FindPropertyRelative("north"));
        EditorGUI.MultiPropertyField(doorNorthRect, labels, property.FindPropertyRelative("north"));

        EditorGUI.EndProperty();
    }
}

