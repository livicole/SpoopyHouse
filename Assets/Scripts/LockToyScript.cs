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
        foreach (Transform myObject in transform.parent)
        {
            //Debug.Log(myObject.name);
            if (myObject.tag == "Door")
            {
                //Debug.Log(myObject.name);
                //myObject.GetComponent<DoorScript>().LockDoor();
            }
        }
    }

    void OnDestroy()
    {
        foreach (Transform myObject in transform.parent)
        {
            if (myObject.tag == "Door")
            {
                //myObject.GetComponent<DoorScript>().ResetDoor();
            }
        }
    }
}