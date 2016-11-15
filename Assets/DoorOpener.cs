using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {

    bool open = false;
    float angle;
    Transform hinge;
    GameObject child;

    public float speed;


	// Use this for initialization
	void Start () {
        child = GameObject.Find("ChildPlayer");
        hinge = transform.GetChild(0);

	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(hinge.localEulerAngles.y);
        
        if (hinge.localEulerAngles.y > 95f)
        {
            angle = -0.1f;
        }
        else
        {
            angle = hinge.localEulerAngles.y;
        }

        if (Input.GetButtonDown("Ghost Button A"))
        {

            Ray rayToPlayer = new Ray(transform.position, child.transform.position - transform.position);
            RaycastHit rayHitToPlayer = new RaycastHit();

            if (Physics.Raycast(rayToPlayer, out rayHitToPlayer))
            {
                if (rayHitToPlayer.collider.name == "ChildPlayer" && Vector3.Distance(child.transform.position, transform.position) <= 2f)
                open = !open;
            }
        }
        //Debug.Log(hinge.localEulerAngles.y);
 

        

        if (open && angle < 90f) 
        {
            hinge.Rotate(0, speed * Time.deltaTime, 0);
        }

        if (!open && angle > 0f)
        {
            hinge.Rotate(0, -speed * Time.deltaTime, 0);
        }

    }
}
