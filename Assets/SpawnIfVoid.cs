using UnityEngine;
using System.Collections;

public class SpawnIfVoid : MonoBehaviour {

    Vector3[] cardinals;
    public LayerMask layermask;


    // Use this for initialization
    void Start () {
        /*
        cardinals = new Vector3[4];
        cardinals[0] = new Vector3(0, 0, 1); //Up direction
        cardinals[1] = new Vector3(1, 0, 0); //Right direction
        cardinals[2] = new Vector3(0, 0, -1); // Down direction
        cardinals[3] = new Vector3(-1, 0, 0);// Left direction
        */
        //Physics.IgnoreLayerCollision(18, 14, true);
        //Physics.IgnoreLayerCollision(18, 12, true);
    }
	
	// Update is called once per frame
	void Update () {
        bool empty = true;
        
            Ray rayDetect = new Ray(transform.position, transform.forward);
            RaycastHit rayHitInfo = new RaycastHit();
            if(Physics.Raycast(rayDetect, out rayHitInfo, 5f))
            {
                Debug.Log("Door next to something: " + rayHitInfo.collider.name);
                empty = false;
            }


        if(empty)
        {
            transform.FindChild("DoubleDoor_Full").FindChild("doubleDoor").GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            transform.FindChild("DoubleDoor_Full").FindChild("doubleDoor").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        //Debug.Log(empty);
    }
}
