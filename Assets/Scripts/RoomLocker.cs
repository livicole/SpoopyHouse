using UnityEngine;
using System.Collections;

public class RoomLocker : MonoBehaviour {

    //This script will prevent the room its in from moving.
    private Transform currentRoom;

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        currentRoom = transform.parent.parent;
        currentRoom.GetComponent<GridLocker>().childLocked = true;
    }

    void OnDestroy()
    {
        currentRoom.GetComponent<GridLocker>().childLocked = false;
    }
    
}
