using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

    Vector3 originalOrientation = new Vector3(0, 0, 0);
    Vector3 originalRotation;

    //public GameObject otherDoor;
    public int priority;
    Transform myChild;
    public GameObject myDoorUI;

    public Transform otherDoor;

    public bool isConnected;
    public bool firstConnect = false;
    public bool isLive = true;
    private float timer = 0, timerEnd = 0.2f;

    public enum Direction { North, East, South, West };

    public Direction location;
    public Vector3 coordinateOfDoor;
    public Transform room;
    public bool placeholder = false;
    public bool locked;

    float doorAdjusment = 1.98f;

    public Transform doorUIprefab;

    GridInfo gridBase;

	// Use this for initialization
	void Start () {
        myChild = transform.GetChild(0) ;
        otherDoor = null;
        originalRotation = transform.localEulerAngles;
        gridBase = GameObject.Find("GridBase").GetComponent<GridInfo>();
        //transform.root.GetComponent<GridLocker>().numDoors++;
        Transform temp = Instantiate(doorUIprefab, transform.position, transform.rotation) as Transform;
        myDoorUI = temp.gameObject;
        temp.GetComponent<DoorUIScript>().myDoor = gameObject;
        priority = GameObject.Find("GameManager").GetComponent<DoorTracker>().index;
        GameObject.Find("GameManager").GetComponent<DoorTracker>().index++;
        //ConnectDoors();
    }
	
	// Update is called once per frame
	void Update () {
        Physics.IgnoreLayerCollision(14, 18, true);

        Vector3 position = room.GetComponent<GridLocker>().CalculateGridToReal(coordinateOfDoor + room.GetComponent<GridLocker>().gridLocation);
        if (location == Direction.North)
        {
            position += new Vector3(gridBase.blockLength / 2, doorAdjusment, gridBase.blockLength);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (location == Direction.East)
        {
            position += new Vector3(gridBase.blockLength, doorAdjusment, gridBase.blockLength / 2);
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
        if (location == Direction.South)
        {
            position += new Vector3(gridBase.blockLength / 2, doorAdjusment, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (location == Direction.West)
        {
            position += new Vector3(0, doorAdjusment, gridBase.blockLength / 2);
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
        transform.position = position;
        if (!firstConnect)
        {
            ConnectDoors();
            firstConnect = true;
        }
        if(otherDoor != null)
        {
            locked = false;
        }
        else
        {
            locked = true;
        }
    }

    //Give us the room in this coordinate, null if none
    public Transform FindRoomInCoordinate(Vector3 coordinate)
    {
        foreach (RoomInfo roomTransform in gridBase.usedGridBlocks)
        {
             if (roomTransform.coordinate == coordinate)
             {
                  if (roomTransform.room != room)
                        return roomTransform.room;
             }
        }
        return null;
    }

    //Searches for the transform of the adjacent coordinate based on direction. Returns null if false;
    Transform FindRelativeRoom(Vector3 coordinate, Direction direction)
    {
        //Debug.Log(coordinate);
        Vector3 adjustmentVector = new Vector3(0, 0, 0);
        if(direction == Direction.North)
        {
            adjustmentVector = new Vector3(0, 0, 1);
        }
        else if(direction == Direction.East)
        {
            adjustmentVector = new Vector3(1, 0, 0);
        }
        else if(direction == Direction.South)
        {
            adjustmentVector = new Vector3(0, 0, -1);
        }
        else if(direction == Direction.West)
        {
            adjustmentVector = new Vector3(-1, 0, 0);
        }

        if(adjustmentVector != Vector3.zero)    
        {
            foreach (RoomInfo roomTransform in gridBase.usedGridBlocks)
            {
                if (roomTransform.coordinate == coordinate + adjustmentVector)
                {
                    if (roomTransform.room != room)
                    return roomTransform.room;
                }
            }
        }
        
        return null;
    }
    
    Transform FindAdjacentDoor(Transform adjacentRoom, Direction directionOfDoor, Vector3 specificCoordinate)
    {
        GridLocker gridLocker = adjacentRoom.GetComponent<GridLocker>();
        foreach(CoordinateInfo coordInfo in gridLocker.coordinatesOccupied)
        {
            if(coordInfo.coordinateOccupied + gridLocker.gridLocation == specificCoordinate)
            {
                if (directionOfDoor == Direction.North)
                {
                    if (coordInfo.doors.south)
                    {
                        return coordInfo.doors.southDoor;
                    }
                }
                if (directionOfDoor == Direction.South)
                {
                    if (coordInfo.doors.north)
                    {
                        return coordInfo.doors.northDoor;
                    }
                }
                if (directionOfDoor == Direction.West)
                {
                    if (coordInfo.doors.east)
                    {
                        return coordInfo.doors.eastDoor;
                    }
                }
                if (directionOfDoor == Direction.East)
                {
                    if (coordInfo.doors.west)
                    {
                        return coordInfo.doors.westDoor;
                    }
                }
            }
        }
        //If this happens there is no door.
        return null;
    }

    //Checks for adjacent rooms for this door. Connects it if there is a relevant door
    public void ConnectDoors()
    {      
        if (GameObject.Find("GhostCursor").GetComponent<CursorController>().connectable)
        {
            //Debug.Log("Door with priority: " + priority);
            Vector3 doorGridLocation = room.GetComponent<GridLocker>().gridLocation + coordinateOfDoor;
            Vector3 doorSearchLocation = new Vector3(0, 0, 0);
            Direction searchOrientation = Direction.North;
            Transform adjacentRoom = null;
            //Use helper function to get releveant room.
            if (location == Direction.North)
            {
                //Debug.Log("North");         
                doorSearchLocation = doorGridLocation + new Vector3(0, 0, 1);
                searchOrientation = Direction.South;
            }
            else if (location == Direction.East)
            {
                //Debug.Log("East");
                adjacentRoom = FindRelativeRoom(doorGridLocation, Direction.East);
                doorSearchLocation = doorGridLocation + new Vector3(1, 0, 0);
                searchOrientation = Direction.West;
            }
            else if (location == Direction.South)
            {
                //Debug.Log("South");
                adjacentRoom = FindRelativeRoom(doorGridLocation, Direction.South);
                doorSearchLocation = doorGridLocation + new Vector3(0, 0, -1);
                searchOrientation = Direction.North;
            }
            else if (location == Direction.West)
            {
                //Debug.Log("West");
                adjacentRoom = FindRelativeRoom(doorGridLocation, Direction.West);
                doorSearchLocation = doorGridLocation + new Vector3(-1, 0, 0);
                searchOrientation = Direction.East;
            }
            adjacentRoom = FindRoomInCoordinate(doorSearchLocation);
                
            DoorTracker doorTracker = GameObject.Find("GameManager").GetComponent<DoorTracker>();
            foreach(ObjectInfo door in doorTracker.doors)
            {
                if (door.obj.GetComponent<DoorScript>().coordinateOfDoor + door.obj.GetComponent<DoorScript>().room.GetComponent<GridLocker>().gridLocation == doorSearchLocation)
                {
                    //Debug.Log("Correct location");
                    if (door.obj.GetComponent<DoorScript>().location == searchOrientation)
                    {
                        ResetThisDoor();
                        Transform adjacentDoor = FindAdjacentDoor(adjacentRoom, location, doorSearchLocation);
                        if (adjacentDoor != null)
                        {
                            DoorScript adjacentDoorScript = adjacentDoor.GetComponent<DoorScript>();
                            otherDoor = adjacentDoor;
                            adjacentDoorScript.otherDoor = transform;
                            isConnected = true;
                            adjacentDoorScript.isConnected = true;
                            if (priority < adjacentDoorScript.priority)
                            {
                                DisableDoor();
                            }
                            else
                            {
                                adjacentDoorScript.DisableDoor();
                            }
                        }
                    }
                }
            }

            /*
            if (room.name == "Placeholder Collection")
            {
                Debug.Log("here fam");
            }

            //No adjacent room if null, return and cease function;
            if (adjacentRoom == null)
            {
                Debug.Log("No adjacent Door: " + room.name);
                ResetDoors();
                return;
            }
            else if (adjacentRoom == room.GetComponent<GridLocker>().copyOf)
            {
                Debug.Log("Same room : " + room.name);
                ResetDoors();
                return;
            }
            else
            {
              
                ResetThisDoor();
                Transform adjacentDoor = FindAdjacentDoor(adjacentRoom, location, doorSearchLocation);
                if (adjacentDoor != null)
                {
                    DoorScript adjacentDoorScript = adjacentDoor.GetComponent<DoorScript>();
                    otherDoor = adjacentDoor;
                    adjacentDoorScript.otherDoor = transform;
                    isConnected = true;
                    adjacentDoorScript.isConnected = true;
                    if (priority < adjacentDoorScript.priority)
                    { 
                        DisableDoor();
                    }
                    else
                    {
                        adjacentDoorScript.DisableDoor();
                    }
                }
            }*/
        }
       
    }

    public void ResetDoors()
    {
        //Debug.Log("Resetting: " + transform.name);
        ResetOtherDoor();
        ResetThisDoor();
    }

    public void ResetThisDoor()
    {
        otherDoor = null;
        //Debug.Log(otherDoor);
        isConnected = false;
        EnableDoor();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ResetOtherDoor()
    {
        if (otherDoor != null)
        {
            otherDoor.gameObject.GetComponent<DoorScript>().ResetThisDoor();
        }
        else { //Debug.Log("Nothing else to reset. Called from : " + transform.name); 
        }
    }

    public void DisableDoor()
    {
        //GetComponent<BoxCollider>().enabled = false;
        //transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        //transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
        //transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
        if(myDoorUI != null)
        {
            //Debug.Log("Disable door UI");
            myDoorUI.GetComponent<Renderer>().enabled = false;
        }
        //otherDoor.GetComponent<DoorScript>().otherDoor = null;
        //otherDoor = null;
        transform.GetChild(0).GetComponent<Collider>().enabled = false;
        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = false;
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        
    }

    public void EnableDoor()
    {
        if(myDoorUI != null)
        {
            myDoorUI.GetComponent<Renderer>().enabled = true;
            //GetComponent<BoxCollider>().enabled = true;
            //transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(0).GetComponent<Collider>().enabled = true;
            transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = true;
            transform.GetChild(1).GetComponent<Renderer>().enabled = true;
            //transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
        }


    }

    public void FixDoorUI(Transform door)
    {
        //door.GetChild(0).GetComponent<Renderer>().enabled = false;
        Transform temp = Instantiate(doorUIprefab);
        temp.GetComponent<DoorUIScript>().myDoor = door.gameObject;
        //temp.GetComponent<DoorUIScript>().overrideOtherUI = true;
        door.GetComponent<DoorScript>().myDoorUI = temp.gameObject;
        door.GetComponent<DoorScript>().priority = 999;

    }

    public void CopyDoor(Transform door)
    {
        DoorScript doorScript = door.GetComponent<DoorScript>();
        priority = doorScript.priority;
    }


}
