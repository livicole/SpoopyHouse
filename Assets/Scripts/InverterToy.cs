using UnityEngine;
using System.Collections;

public class InverterToy : MonoBehaviour {

    GameObject theChild;
    bool activate;


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
                theChild.GetComponent<PlayerMovement>().invert = true;
                theChild.transform.GetChild(0).GetComponent<CameraController>().invert = true;
            }
        }


    }

    void OnDestroy()
    {
        theChild.GetComponent<PlayerMovement>().invert = false;
        theChild.transform.GetChild(0).GetComponent<CameraController>().invert = false;
    }
}
