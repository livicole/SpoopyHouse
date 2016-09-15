using UnityEngine;
using System.Collections;

public class ChildRemover : MonoBehaviour {

    Transform flashlight;

    [SerializeField]
    float detectDistance;

    [SerializeField]
    float destroyCooldown;

    private float destroyTimer;
    private bool coolingdown;

	// Use this for initialization
	void Start () {
        flashlight = GameObject.Find("Flashlight").transform;
	}
	
	// Update is called once per frame
	void Update () {

        if (coolingdown)
        {
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= destroyCooldown)
            {
                destroyTimer = 0;
                coolingdown = false;
            }
        }


        if (Input.GetButton("Use"))
        {
            if (!coolingdown)
            {
                //Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y - heightDifference, transform.position.z);
                Vector3 rayPosition = flashlight.transform.position;
                Vector3 rayDirection = flashlight.transform.forward;
                Ray forwardRay = new Ray(rayPosition, rayDirection);
                RaycastHit forwardRayHit = new RaycastHit();
                Debug.DrawRay(rayPosition, rayDirection * detectDistance, Color.red);

                if (Physics.Raycast(forwardRay, out forwardRayHit, detectDistance))
                {
                    Debug.Log(forwardRayHit.collider.gameObject.layer);
                    if (forwardRayHit.collider.gameObject.layer == 11)
                    {
                        coolingdown = true;
                        Destroy(forwardRayHit.collider.gameObject);
                    }
                }
            }
            else
            {
                Debug.Log("On Cooldown");
            }
        }
    }
}
