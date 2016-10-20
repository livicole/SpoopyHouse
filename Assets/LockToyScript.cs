using UnityEngine;
using System.Collections;

public class LockToyScript : MonoBehaviour {

    bool done = false;
    


	void Update () {
        if (!done)
        {
            done = true;
            LockDoors();
        }
	
	}

    void LockDoors()
    {
        foreach (Transform myObject in transform.parent)
        {
            if (myObject.tag == "Door")
            {
                Debug.Log(myObject.name);
                myObject.GetComponent<DoorScript>().LockDoor();
            }
        }
    }

    void OnDestroy()
    {
        foreach (Transform myObject in transform.parent)
        {
            if (myObject.tag == "Door")
            {
                myObject.GetComponent<DoorScript>().ResetDoors();
            }
        }
    }
}
