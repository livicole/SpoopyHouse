using UnityEngine;
using System.Collections;

public class DoorUIScript : MonoBehaviour {


    public GameObject myDoor;
    [HideInInspector]
    public bool overrideOtherUI = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(myDoor == null)
        {
            Destroy(gameObject);
        }
        else
        {
            if (!overrideOtherUI)
            {

                transform.position = new Vector3(myDoor.transform.position.x, 12f, myDoor.transform.position.z);
            }
            else
            {
                transform.position = new Vector3(myDoor.transform.position.x, 14f, myDoor.transform.position.z);
            }
            transform.rotation = myDoor.transform.rotation;
            if (myDoor != null)
            {
                if (myDoor.GetComponent<DoorScript>().otherDoor != null)
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
