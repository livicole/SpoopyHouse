using UnityEngine;
using System.Collections;

public class LockToyScript : MonoBehaviour {

    bool done = false;
    float timer;
    


	void Update () {
        if (!done)
        {
            //Debug.Log("Hello?");
            done = true;
            LockDoors();
        }
        if (timer >= 1500)
        {
            Destroy(this.gameObject);
        }
        timer++;
        
	}

    void LockDoors()
    {
        foreach (Transform myObject in transform.root.GetComponent<GridLocker>().doors)
        {
            myObject.GetComponent<DoorScript>().lockedOverride = true;
            myObject.GetComponent<DoorScript>().locked = true;
            myObject.GetComponent<DoorScript>().otherDoor.GetComponent<DoorScript>().lockedOverride = true;
            myObject.GetComponent<DoorScript>().otherDoor.GetComponent<DoorScript>().locked= true;

        }
    }

    void OnDestroy()
    {

        foreach (Transform myObject in transform.root.GetComponent<GridLocker>().doors)
        {
            myObject.GetComponent<DoorScript>().lockedOverride = false;
            myObject.GetComponent<DoorScript>().otherDoor.GetComponent<DoorScript>().lockedOverride = false;

        }
    }
}