using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

    Vector3 originalOrientation = new Vector3(-0.04f, -5.1f, -0.29652f);

    //public GameObject otherDoor;
    public int priority;
    Transform myChild;

    public Transform otherDoor;

    private bool check = false;
    private float timer = 0, timerEnd = 0.2f;

	// Use this for initialization
	void Start () {
        myChild = transform.GetChild(1) ;
        otherDoor = null;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if(setup && name == "DoubleDoor_Full (12)")
        {
            if (timer < timerEnd)
            {
                timer += Time.deltaTime;
            }
            else
            {
                ResetDoors();
                setup = false;
            }
           
        }*/
        Physics.IgnoreLayerCollision(14, 18, true);
    }

    void OnTriggerStay(Collider col)
    {
        if (check)
        {
            
            check = false;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        
        if (col.transform.parent.tag == "Door")
        {
            Debug.Log("I am" + name + " colliding with " + col.transform.parent.transform.name);
            if (priority > col.GetComponentInParent<DoorScript>().priority)
            {
                if (col.GetComponentInParent<DoorScript>().otherDoor != null)
                {
                    Debug.Log("ERROR! Reading an old other door. Replacing....");
                    col.GetComponentInParent<DoorScript>().otherDoor.GetComponent<DoorScript>().ResetThisDoor();
                    col.GetComponentInParent<DoorScript>().otherDoor.GetComponent<DoorScript>().check = true;
                }
                otherDoor = col.transform.parent.transform;
                col.GetComponentInParent<DoorScript>().otherDoor = transform;
                Debug.Log("I am " + gameObject.name + " with P" + priority + " being turned off by " + col.transform.parent.name + " with P" + priority);
                col.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Incorrect object: " + col.transform.parent.name);
            }
        }
    }

    public void ResetDoors()
    {
       // Debug.Log("Resetting: " + transform.name);
        ResetOtherDoor();
        ResetThisDoor();

    }

    public void ResetThisDoor()
    {
        Debug.Log("Resetting " + name);
        gameObject.SetActive(true);
        transform.GetChild(1).transform.localPosition = originalOrientation;
        transform.GetChild(1).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ResetOtherDoor()
    {
        if (otherDoor != null)
        {
            otherDoor.gameObject.GetComponent<DoorScript>().ResetThisDoor();
            Debug.Log("Also resetting: " + otherDoor.name);
        }
        else { Debug.Log("Nothing else to reset. Called from : " + transform.name); }
    }

    public void LockDoor()
    {
        transform.GetChild(1).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

}
