using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class VortexManipulator : MonoBehaviour {

    private Transform childCamera;
    public bool on = false;
    public float maxAngle = 80f;

    private Transform theChild;

    // Use this for initialization
    void Start () {
        childCamera = GameObject.Find("ChildCamera").transform;
        theChild = childCamera.parent;
	}
	
	// Update is called once per frame
	void Update () {
        Ray rayToPlayer = new Ray(transform.position, theChild.transform.position - transform.position);
        RaycastHit rayToPlayerHit = new RaycastHit();
        Debug.DrawRay(transform.position, theChild.transform.position - transform.position);

        if (Physics.Raycast(rayToPlayer, out rayToPlayerHit))
        {
            if (rayToPlayerHit.collider.name == "ChildPlayer")
            {
                on = true;
            }
        }

        if (on)
        {
            //Distance to player, capped from 0 to 10 units.
            float distanceToPlayer = Mathf.Clamp(Vector3.Distance(transform.position, theChild.transform.position), 0, 10);
            maxAngle = distanceToPlayer * 8; //From 0 - 80 maxangle.

            childCamera.rotation = Quaternion.Euler(childCamera.rotation.x, childCamera.rotation.y, 180);
        }

    }

    void OnDestroy()
    {
        childCamera.GetComponent<Vortex>().OnResetVision();
    }
       
}
