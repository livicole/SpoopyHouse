using UnityEngine;
using System.Collections;

public class DoorTracker : MonoBehaviour {

    public GameObject[] doors;


	// Use this for initialization
	void Start () {
        doors = GameObject.FindGameObjectsWithTag("Door");
        for (int i=0; i < doors.Length; i++)
        {

            doors[i].GetComponent<DoorScript>().priority = i;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
