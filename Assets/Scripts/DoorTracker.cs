using UnityEngine;
using System.Collections;

public class DoorTracker : MonoBehaviour {

    public GameObject[] doors, rooms;

    public bool[] doorChecked, roomChecked;
    public GameObject doorUI;
    private Transform child;


	// Use this for initialization
	void Start () {
        doors = GameObject.FindGameObjectsWithTag("Door");
        rooms = GameObject.FindGameObjectsWithTag("Room");
        child = GameObject.Find("ChildPlayer").transform;
        doorChecked = new bool[doors.Length];
        roomChecked = new bool[rooms.Length];
        for (int i=0; i < doors.Length; i++)
        {
            GameObject newDoorUI = Instantiate(doorUI);
            newDoorUI.GetComponent<DoorUIScript>().myDoor = doors[i];
            doors[i].GetComponent<DoorScript>().priority = i;
            doors[i].GetComponent<DoorScript>().myDoorUI = newDoorUI;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool AreAllRoomsConnected()
    {

        int index = GetRoomIndex(child.GetComponent<RoomParenter>().currentRoom);
        //Debug.Log(index);
        //Debug.Log(rooms[index]);
        //rooms[index]
        CheckDoorsInRoom(rooms[index].transform);


        return CheckAllRooms();

    }

    public void CheckDoorsInRoom(Transform room)
    {
        foreach(Transform door in room.GetComponent<GridLocker>().doors)
        {
            if (!doorChecked[GetDoorIndex(door)])
            {
                //Debug.Log("Checking door: " + door);
                doorChecked[GetDoorIndex(door)] = true;
                if (door.GetComponent<DoorScript>().otherDoor != null)
                {
                    CheckDoorsInRoom(door.GetComponent<DoorScript>().otherDoor.root);
                }
            }
        }
        roomChecked[GetRoomIndex(room)] = true;
    }

    public bool CheckAllRooms()
    {
        foreach (bool myBool in roomChecked)
        {
            if (!myBool)
            {
                return false;
            }
        }
        return true;
    }

    public int GetDoorIndex(Transform door)
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (door.Equals(doors[i].transform))
            {
                return i;
            }
        }
        return -100;
    }

    //Returns index of this transform within the array
    public int GetRoomIndex(Transform room)
    {
        for(int i = 0; i < rooms.Length; i++)
        {
            if (room.Equals(rooms[i].transform))
            {
                return i;
            }
        }
        return -100;
    }

    public void ResetAllBools()
    {
        for(int i = 0; i < roomChecked.Length; i++)
        {
            roomChecked[i] = false;
        }

        for(int i = 0; i < doorChecked.Length; i++)
        {
            doorChecked[i] = false;
        }
    }
}
