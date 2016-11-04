using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomParenter : MonoBehaviour {

    private Transform currentRoom;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray verticalRay = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit verticalInfo = new RaycastHit();

        if(Physics.Raycast(verticalRay, out verticalInfo, 100f))
        {
            //Debug.Log(verticalInfo.collider.gameObject.layer);
            if(transform.name == "ChildPlayer")
            {
                Debug.Log(currentRoom);
                if(currentRoom != null)
                {
                    if (verticalInfo.collider.transform.parent.parent != currentRoom)
                    {
                        currentRoom.GetComponent<GridLocker>().childLocked = false;
                    }
                }
              
                currentRoom = verticalInfo.collider.transform.parent.parent;
                currentRoom.GetComponent<GridLocker>().childLocked = true;
            }
            else if(verticalInfo.collider.gameObject.layer == 10)
            {
                Transform temp = verticalInfo.collider.transform;
                while(temp.gameObject.layer != 14)
                {
                    //Debug.Log("Changing parent from: " + temp.name);
                    temp = temp.parent;
                    
                }
                //temp = temp.parent; //One more from the RoomFiller to parent.

                transform.parent = temp;
            }
            else if(verticalInfo.collider.gameObject.name == "GridBase" && transform.name == "ChildPlayer")
            {
                //GameObject.Find("Defeat").GetComponent<Text>().text = "Child Escaped!";
            }

            if (transform.name == "GravityToy(Clone)")
            {
                GetComponent<GravityToyScript>().hasSetParent = true;
            }
            else if (transform.tag == "Lamp" || transform.tag == "Toys")
            {
                currentRoom = transform.parent.parent;
                currentRoom.GetComponent<GridLocker>().childLocked = true;
            }
        }
	}

    void OnDestroy()
    {
        if(transform.tag == "Lamp" || transform.tag == "Toys")
        {
            currentRoom.GetComponent<GridLocker>().childLocked = false;
        }
    }
}
