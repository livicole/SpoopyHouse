using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {

    bool open = false;
    float angle;
    GameObject flashlight;

    public float speed;

	AudioSource soundManager;
	public AudioClip doorOpen, doorClose;


	// Use this for initialization
	void Start () {
		soundManager = GameObject.Find ("SoundManager").GetComponent<AudioSource>();
        flashlight = GameObject.Find("Flashlight");
       

	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(hinge.localEulerAngles.y);
        
		if (transform.localEulerAngles.y > 95f)
        {
            angle = -0.1f;
        }
        else
        {
            angle = transform.localEulerAngles.y;
        }

        if (Input.GetButtonDown("Ghost Button A"))
        {
			//Debug.Log ("press");
		
			Ray rayToPlayer = new Ray(transform.position, flashlight.transform.position - transform.position);
            RaycastHit rayHitToPlayer = new RaycastHit();

            if (Physics.Raycast(rayToPlayer, out rayHitToPlayer))
            {
				if (rayHitToPlayer.collider.name == "ChildPlayer" && Vector3.Distance (flashlight.transform.position, transform.position) <= 3f) {
					open = !open;
					if (open) {
						soundManager.PlayOneShot (doorOpen, 1.0f);
					} else {
						soundManager.PlayOneShot (doorClose, 1.0f);
					}
				}
					
            }
        }
        //Debug.Log(hinge.localEulerAngles.y);
 

        

        if (open && angle < 90f) 
        {
			//add open sound

            transform.Rotate(0, speed * Time.deltaTime, 0);
        }

        if (!open && angle > 0f)
        {
			//add close sound
            transform.Rotate(0, -speed * Time.deltaTime, 0);
        }

    }
}
