using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

    Vector3 originalOrientation = new Vector3(-0.04f, -5.1f, -0.29652f);

    public Transform otherDoor;
    Transform doubleDoorChild;
    Transform doorFrameChild;
    Transform doorHinge;
    public GameObject myDoorUI;

    public bool isConnected = false;
    
    public int priority;
    

	// Use this for initialization
	void Start () {
        doubleDoorChild = transform.GetChild(0).GetChild(0);
        doorFrameChild = transform.GetChild(1);
        doorHinge = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
        Physics.IgnoreLayerCollision(14, 18, true);


    }


    void OnTriggerEnter(Collider col)
    {
        Debug.Log("hey");
        if (col.name == "DoorHinge")
        {
            
            if (col.transform.parent.name != name)
            {
                //Debug.Log("eheyy");
                //Debug.Log("I am " + name + ". P:" + priority + " colliding with " + col.transform.parent.name + ". P:" + col.transform.parent.GetComponent<DoorScript>().priority);
                otherDoor = col.transform.parent;
                col.transform.parent.GetComponent<DoorScript>().otherDoor = transform;
                //if my priority is greater: lock this door
                //if my priority is lower: disable this door
                if (priority > otherDoor.GetComponent<DoorScript>().priority)
                {
                    UnlockDoor();
                    isConnected = true;
                    otherDoor.GetComponent<DoorScript>().isConnected = true;
                    otherDoor.GetComponent<DoorScript>().HideDoor();   
                }
            }
        }
        
    }



    public void ResetDoor()
    {
        isConnected = false;
        LockDoor();
        ShowDoor();
        myDoorUI.GetComponent<Renderer>().material.color = Color.red;
        if (otherDoor != null)
        {
            otherDoor.GetComponent<DoorScript>().isConnected = false;
            otherDoor.GetComponent<DoorScript>().LockDoor();
            otherDoor.GetComponent<DoorScript>().ShowDoor();
        }

    }

    public void HideDoor()
    {
        GetComponent<BoxCollider>().enabled = false;
        doorFrameChild.GetComponent<MeshRenderer>().enabled = false;
        doubleDoorChild.GetComponent<MeshRenderer>().enabled = false;
        doorHinge.GetComponent<BoxCollider>().enabled = false;
        myDoorUI.GetComponent<MeshRenderer>().enabled = false;
    }

    public void ShowDoor()
    {
        GetComponent<BoxCollider>().enabled = true;
        doorFrameChild.GetComponent<MeshRenderer>().enabled = true;
        doubleDoorChild.GetComponent<MeshRenderer>().enabled = true;
        doorHinge.GetComponent<BoxCollider>().enabled = true;
        myDoorUI.GetComponent<MeshRenderer>().enabled = true;
    }

    public void LockDoor()
    {
        doubleDoorChild.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnlockDoor()
    {
        doubleDoorChild.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        myDoorUI.GetComponent<Renderer>().material.color = Color.green;
    }

}