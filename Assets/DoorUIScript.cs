using UnityEngine;
using System.Collections;

public class DoorUIScript : MonoBehaviour {


    public GameObject myDoor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(myDoor.transform.position.x, 10.6f, myDoor.transform.position.z);
        transform.rotation = myDoor.transform.rotation;
	}
}
