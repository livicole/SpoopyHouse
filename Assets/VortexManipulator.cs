using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class VortexManipulator : MonoBehaviour {

    private Transform childCamera;
    public bool on = false;
    public Vector2 radius = new Vector2(0.4f, 0.4f);
    public float angle;
    public Vector2 center;

    private float rate = 0;
    private float timer = 0;
    public bool switched = false;
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

        if (Physics.Raycast(rayToPlayer, out rayToPlayerHit, 1000f))
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
            timer += Time.deltaTime;
            center = new Vector2(0.5f, 0.5f + 0.4f * Mathf.Sin(2.5f * timer));
            if (!switched)
            {
                angle = Mathf.Lerp(maxAngle, -maxAngle, rate);
                rate += 0.2f * Time.deltaTime;
                if (angle < -(maxAngle - 1))
                {
                    angle = -maxAngle;
                }
                if (angle == -maxAngle)
                {
                    switched = true;
                    rate = 0;
                }

            }
            else
            {
                angle = Mathf.Lerp(-maxAngle, maxAngle, rate);
                rate += 0.2f * Time.deltaTime;
                if (angle > maxAngle - 1)
                {
                    angle = maxAngle;
                }
                if (angle == maxAngle)
                {
                    switched = false;
                    rate = 0;
                }

            }
        }

        childCamera.GetComponent<Vortex>().angle = angle;
        childCamera.GetComponent<Vortex>().radius = radius;
        childCamera.GetComponent<Vortex>().center = center;


    }

    void OnDestroy()
    {
        childCamera.GetComponent<Vortex>().OnResetVision();
    }
       
}
