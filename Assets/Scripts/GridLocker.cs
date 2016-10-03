using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridLocker : MonoBehaviour {

    
    Transform gridBase;

    [SerializeField]
    float height;

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


	// Use this for initialization
	void Start () {
        gridBase = GameObject.Find("GridBase").transform;
        currentLocation = CalculateGridToReal(gridLocation);
        foreach (Vector3 takenBlock in coordinatesOccupied)
        {
            gridInfo.AddBlock(takenBlock + gridLocation, roomNumber);
        }
        //adjustmentVector = transform.FindChild("RoomFiller").localScale / 2;
        //adjustmentVector = new Vector3(adjustmentVector.z, 0, adjustmentVector.x);
        Debug.Log(adjustmentVector);
        originalRoomFillerPosition = transform.FindChild("RoomFiller").localPosition;

       
        gridInfo = gridBase.GetComponent<GridInfo>();
    }

    // Update is called once per framsdse
    void Update()
    {
        gridBase = GameObject.Find("GridBase").transform;
        gridInfo = gridBase.GetComponent<GridInfo>();
        //adjustmentVector = transform.FindChild("RoomFiller").localScale / 2;
        //3adjustmentVector = new Vector3(adjustmentVector.z, 0, adjustmentVector.x);
        //Debug.Log(gridBase.GetComponent<GridInfo>().gridMax);


        //Move pivot on rotation.   
        float rotationY = transform.eulerAngles.y;

        if (rotationY == 0)
        {
            //Debug.Log("No rotation.");
            transform.FindChild("RoomFiller").localPosition = originalRoomFillerPosition;
        }
        else if (rotationY == 90)
        {
            //Debug.Log("detected rotation");
            Vector3 newFillerPosition = originalRoomFillerPosition;
            Vector3 fillerScale = transform.FindChild("RoomFiller").localScale;
            newFillerPosition += new Vector3(-(fillerScale.x), 0, 0);
            transform.FindChild("RoomFiller").localPosition = newFillerPosition;
        }
        else if (rotationY == 180)
        {
            Vector3 newFillerPosition = originalRoomFillerPosition;
            newFillerPosition = new Vector3(-newFillerPosition.x, newFillerPosition.y, -newFillerPosition.z);
            transform.FindChild("RoomFiller").localPosition = newFillerPosition;
        }
        else if (rotationY == 270)
        {
            Vector3 newFillerPosition = originalRoomFillerPosition;
            Vector3 fillerScale = transform.FindChild("RoomFiller").localScale;
            newFillerPosition += new Vector3(0, 0, -fillerScale.z);
            transform.FindChild("RoomFiller").localPosition = newFillerPosition;
        }

        //Check if gridLocation is not where our real position is AND we are supposed to be locked, recalculate grid position.
        if (CalculateGridToReal(gridLocation) != transform.position && locked)
        {
            currentLocation = CalculateGridToReal(gridLocation);
            Debug.Log(CalculateGridToReal(gridLocation));
            transform.position = currentLocation;

            //currentRoomFillerPosition = transform.FindChild("RoomFiller").localPosition;
        }
        //While we aren't locked, constantly check if we can move the block to new coordinates.
        if (!locked)
        {
            
            Vector3 position = CheckAvailableOrigin(CalculateRealToGrid(transform.position), gridLocation);
            if(CheckFullAvailability(coordinatesOccupied))
            {
                //Debug.Log("All necessary blocks are available.");
                gridInfo.RemoveBlock(gridLocation, roomNumber);
                gridLocation = position;
                //gridInfo.usedGridBlocks.Add(gridLocation);
                rotation = transform.rotation.eulerAngles.y;
            }
            else
            {
                //Debug.Log("Place in old position: " + gridLocation + " with old rotation: " + rotation);
                Vector3 oldRotation = new Vector3(transform.eulerAngles.x, rotation, transform.eulerAngles.z);
                float rotationDifference = transform.eulerAngles.y - rotation;
                if(rotationDifference == -90)
                {
                    UpdateCoordinates(90);
                }
                else if(rotationDifference == 90)
                {
                    UpdateCoordinates(-90);
                }
                transform.eulerAngles = oldRotation;
            }
        }
    }

    public void UpdateCoordinates(float rotation)
    {
        List<Vector3> newList = new List<Vector3>();
        foreach (Vector3 coordinate in coordinatesOccupied)
        {
            Vector3 newCoordinate = RotatePositionByOrigin(coordinate, rotation);
            newList.Add(newCoordinate);
        }

        float lowestX = 0;
        float lowestZ = 0;
        //Find the bottom left coordinate.
        foreach (Vector3 coordinate in newList)
        {
            if(coordinate.x < lowestX)
            {
                lowestX = coordinate.x;
            }
            if(coordinate.z < lowestZ)
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
            if(tempCoordinate.x < 0 || tempCoordinate.y < 0 || tempCoordinate.z < 0)
            {
                Debug.Log("ERROR!!! Shouldn't allow negative coordinate values!");
            }
            tempList.Add(tempCoordinate);
        }

        coordinatesOccupied = tempList;
    }

    public void UpdateNewBlocks()
    {
        foreach(Vector3 temp in coordinatesOccupied)
        {
            Debug.Log("Adding: " + (temp + gridLocation));
            gridInfo.AddBlock(temp + gridLocation, roomNumber);
        }
    }

    //Remove all of our blocks from the gridInfo.
    public void ClearOldBlocks()
    {
        foreach(Vector3 temp in coordinatesOccupied)
        {
            Debug.Log("Removing: " + (temp + gridLocation));
            gridInfo.RemoveBlock(temp + gridLocation, roomNumber);
        }
    }
    
    //Readjust coordinates to correspond with rotation, either 90 or -90.
    public Vector3 RotatePositionByOrigin(Vector3 coordinate, float rotation)
    {
        float posX = coordinate.x;
        float posY = coordinate.y;
        float posZ = coordinate.z;
        Vector3 newCoordinate = new Vector3(0, 0, 0);
        if (rotation == 90)//Rotate clockwise
        {
            newCoordinate = new Vector3(posZ, posY, -posX);
        }
        else if(rotation == -90)//Rotate counter-clockwise
        {
            newCoordinate = new Vector3(-posZ, posY, posX);
        }
        return newCoordinate;
    }

    //Check that all offshoot blocks won't be placed into a used block.
    public bool CheckFullAvailability(List<Vector3> allCoordinates)
    {
        foreach(Vector4 location in gridInfo.GetComponent<GridInfo>().usedGridBlocks)
        {
            foreach(Vector3 takenBlock in allCoordinates)
            {
                Vector3 coordinate = new Vector3(location.x, location.y, location.z);

                if (coordinate.Equals(takenBlock + CalculateRealToGrid(transform.position)))
                {
                    //Debug.Log("Offshoot block: " + (takenBlock + CalculateRealToGrid(transform.position)) + " can't be moved here: " + location);
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
        foreach (Vector3 location in gridInfo.GetComponent<GridInfo>().usedGridBlocks)
        {
            if (location.Equals(newCoordinates))
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
        location = new Vector3(coordinates.x * 20, height, coordinates.z * 20);
        return location;
    }

    public Vector3 CalculateRealToGrid(Vector3 position)
    {
        Vector3 coordinates;
        gridInfo = gridBase.GetComponent<GridInfo>();
        gridXPosition = Mathf.Clamp((int)(position.x / 20), gridInfo.gridMin, gridInfo.gridMax);
        gridYPosition = Mathf.Clamp((int)(position.z / 20), gridInfo.gridMin, gridInfo.gridMax);
        coordinates = new Vector3(gridXPosition, height, gridYPosition); 

        return coordinates;
    }
    
}
