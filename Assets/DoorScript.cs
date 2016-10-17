using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

    public GameObject otherDoor;
    public int priority;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

	}


    void OnTriggerEnter(Collider col)
    {
        if (col.name == "doubleDoor")
        {
            Debug.Log("found a door");
            if (priority > col.GetComponentInParent<DoorScript>().priority)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void ReactivateDoor()
    {
        gameObject.SetActive(true);
    }
}
