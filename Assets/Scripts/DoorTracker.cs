using UnityEngine;
using System.Collections;

public class DoorTracker : MonoBehaviour {

    public GameObject[] doors;
    public GameObject doorUI;


	// Use this for initialization
	void Start () {
        doors = GameObject.FindGameObjectsWithTag("Door");
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
}
