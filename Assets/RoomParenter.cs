using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomParenter : MonoBehaviour {

    

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
            if(verticalInfo.collider.gameObject.layer == 10)
            {
                Transform temp = verticalInfo.collider.transform;
                while(temp.gameObject.layer != 14)
                {
                    temp = temp.parent;
                    //Debug.Log("Changing parent...");
                }
                temp = temp.parent; //One more from the RoomFiller to parent.

                transform.parent = temp;
            }
            else if(verticalInfo.collider.gameObject.name == "GridBase" && transform.name == "ChildPlayer")
            {
                GameObject.Find("Defeat").GetComponent<Text>().text = "Child Escaped!";
            }

            if (transform.name == "GravityToy")
            {
                GetComponent<GravityToyScript>().hasSetParent = true;
            }
        }
	}
}
