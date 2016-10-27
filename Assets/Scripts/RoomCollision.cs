using UnityEngine;
using System.Collections;

public class RoomCollision : MonoBehaviour {

    private CursorController cursor;
    [HideInInspector]
    public GameObject otherRoom;
    [HideInInspector]
    public Vector3 targetGrid = new Vector3(0, 0, 0);
    Vector3[] cardinals;


    void Start()
    {
        cursor = GameObject.Find("GhostCursor").GetComponent<CursorController>();
        cardinals = new Vector3[4];
        cardinals[0] = new Vector3(0, 0, 1); //Up direction
        cardinals[1] = new Vector3(1, 0, 0); //Right direction
        cardinals[2] = new Vector3(0, 0, -1); // Down direction
        cardinals[3] = new Vector3(-1, 0, 0);// Left direction

    }

    void Update()
    {
        cursor = GameObject.Find("GhostCursor").GetComponent<CursorController>();
        /**
        if (otherRoom != null)
        {
            //Get otherRoom's coordinates.
            targetGrid = otherRoom.transform.parent.GetComponent<GridLocker>().gridLocation;

            //Find the side you are in relation to the other room.
            Vector3 directionToRoom = -(otherRoom.transform.parent.position - transform.parent.position).normalized;
            //Debug.Log("Direction to room: " + directionToRoom);
            int direction = 0; Vector3 currentDifference = new Vector3(999, 999, 999);
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
            //Up direction
            targetGrid += cardinals[direction];
            if(targetGrid.x < 0 || targetGrid.y < 0 || targetGrid.z < 0)
            {
                //Invalid location.
                otherRoom = null;
                targetGrid = new Vector3(0, 0, 0);
            }
        }**/
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == 14)
        {
            //Debug.Log("Room Collision!");
            otherRoom = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.layer == 14)
        {
            //Debug.Log("Room Exit");
            otherRoom = null;
        }
    }
}
