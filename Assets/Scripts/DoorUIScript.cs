using UnityEngine;
using System.Collections;

public class DoorUIScript : MonoBehaviour {


    public GameObject myDoor;
    [HideInInspector]
    public bool overrideOtherUI = false;
    bool retrievedBool = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(myDoor == null)
        {
            //Destroy(gameObject);
        }
        else
        {
            //Retrive the bool only once. No need to continuosly retrive it (optimization)
            if(!retrievedBool)
            {
                overrideOtherUI = myDoor.GetComponent<DoorScript>().placeholder;
                retrievedBool = true;
            }
            
            //Override causes the UI to be above the other UI around it. Giving it priority for the GhostCam
            if (!overrideOtherUI)
            {

                transform.position = new Vector3(myDoor.transform.position.x, 12f, myDoor.transform.position.z);
            }
            else
            {
                transform.position = new Vector3(myDoor.transform.position.x, 14f, myDoor.transform.position.z);
            }

            //Alligning the rotation to its door's rotation
            transform.rotation = myDoor.transform.rotation;

            //As long as we have a door, either make it yellow because it is a placeholder door, green because it is connected, or red because it isn't connected
            if (myDoor != null)
            {
                if (overrideOtherUI)
                {
                    GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if (myDoor.GetComponent<DoorScript>().otherDoor != null)
                {
                    GetComponent<Renderer>().material.color = Color.green;
                }
                else
                {
                    GetComponent<Renderer>().material.color = Color.red;
                }
            }
        } 
	}
}
