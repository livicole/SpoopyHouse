using UnityEngine;
using System.Collections;

public class SpawnIfVoid : MonoBehaviour {

    Vector3[] cardinals;


    // Use this for initialization
    void Start () {
        /*
        cardinals = new Vector3[4];
        cardinals[0] = new Vector3(0, 0, 1); //Up direction
        cardinals[1] = new Vector3(1, 0, 0); //Right direction
        cardinals[2] = new Vector3(0, 0, -1); // Down direction
        cardinals[3] = new Vector3(-1, 0, 0);// Left direction
        */
    }
	
	// Update is called once per frame
	void Update () {
        bool empty = true;
        
            Ray rayDetect = new Ray(transform.position, transform.forward);
            RaycastHit rayHitInfo = new RaycastHit();
            if(Physics.Raycast(rayDetect, out rayHitInfo, 5f))
            {
                Debug.Log(rayHitInfo.collider.name);
                empty = false;
            }


        if(empty)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        Debug.Log(empty);
    }
}
