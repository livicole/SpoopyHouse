using UnityEngine;
using System.Collections;

public class DoorTracker : MonoBehaviour {

    public GameObject[] doors = new GameObject[10];


	// Use this for initialization
	void Start () {
        doors = GameObject.FindGameObjectsWithTag("Door");
        for (int i=0; i<10; i++)
        {
            doors[i].GetComponent<DoorScript>().priority = i;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
