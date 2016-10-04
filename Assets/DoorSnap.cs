using UnityEngine;
using System.Collections;

public class DoorSnap : MonoBehaviour {

    public bool locked, held;
    Vector3[] cardinals;
    Vector3 targetLocation;


	// Use this for initialization
	void Start () {
        cardinals = new Vector3[4];
        Vector3 targetLocation = new Vector3(0, 0, 0);
        cardinals[0] = new Vector3(0, 0, 1); //Up direction
        cardinals[1] = new Vector3(1, 0, 0); //Right direction
        cardinals[2] = new Vector3(0, 0, -1); // Down direction
        cardinals[3] = new Vector3(-1, 0, 0);// Left direction

    }
	
	// Update is called once per frame
	void Update () {
        transform.parent.position = new Vector3(transform.parent.position.x, 5, transform.parent.position.z);
        transform.parent.position = targetLocation;
    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log(col);
        //Make sure its a detector and its not our own detector
        if(col.gameObject.name.Contains("Room") && !held)
        {
            //Find the side you are in relation to the other room.
            Vector3 directionToRoom = -(col.gameObject.transform.position - transform.parent.position).normalized;
            //Debug.Log("Direction to room: " + directionToRoom);
            int direction = 0; Vector3 currentDifference = new Vector3(999, 999, 999);
            for(int i = 0; i < 4; i++)
            {
                Vector3 difference = directionToRoom - cardinals[i];
                //Debug.Log("Difference: " + difference);
                if (difference.magnitude < currentDifference.magnitude)
                {
                    currentDifference = difference;
                    direction = i;
                }
            }
            //Debug.Log("Direction is: " + direction);
           
            if(direction == 0)
            {
                float newZ = col.gameObject.transform.position.z + col.transform.FindChild("RoomFiller").transform.localScale.z;
                targetLocation = new Vector3(col.gameObject.transform.position.x, 5, newZ);
            }
            else if(direction == 1)
            {
                float newX = col.gameObject.transform.position.x + col.transform.FindChild("RoomFiller").transform.localScale.x;
                targetLocation = new Vector3(newX, 5, col.gameObject.transform.position.z);
            }
            else if(direction == 2)
            {
                float newZ = col.gameObject.transform.position.z - col.transform.FindChild("RoomFiller").transform.localScale.z;
                targetLocation = new Vector3(col.gameObject.transform.position.x, 5, newZ);
            }
            else if(direction == 3)
            {
                float newX = col.gameObject.transform.position.x - col.transform.FindChild("RoomFiller").transform.localScale.x;
                targetLocation = new Vector3(newX, 5, col.gameObject.transform.position.z);
            }
            Debug.Log(targetLocation);
            transform.parent.position = targetLocation;
            
            //transform.parent.position = col.gameObject.transform.position;
            
        }
    }
}
