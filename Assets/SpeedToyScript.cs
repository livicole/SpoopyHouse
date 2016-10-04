using UnityEngine;
using System.Collections;

public class SpeedToyScript : MonoBehaviour {

    GameObject theChild;

	// Use this for initialization
	void Start () {

        theChild = GameObject.Find("ChildPlayer");


    }
	
	// Update is called once per frame
	void Update () {

        Ray rayToPlayer = new Ray(transform.position, theChild.transform.position - transform.position);
        RaycastHit rayToPlayerHit = new RaycastHit();
        
        if (Physics.Raycast(rayToPlayer, out rayToPlayerHit))
        {
            if (rayToPlayerHit.collider.name == "ChildPlayer")
            {
                theChild.GetComponent<PlayerMovement>().walkingSpeed = .05f;
                theChild.GetComponent<PlayerMovement>().turningSpeed = 1f;
            }
        }

	}

    void OnDestroy()
    {
        Debug.Log("destroying speed toy");
        theChild.GetComponent<PlayerMovement>().walkingSpeed = .1f;
        theChild.GetComponent<PlayerMovement>().turningSpeed = 2f;
    }

}
