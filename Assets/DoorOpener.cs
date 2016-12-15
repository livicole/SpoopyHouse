using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {

    bool open = false;
    bool opening = false;
    bool closing = false;
    float angle;
    GameObject flashlight;
    Transform hinge;

    bool timer = false;

    public float speed;

    public float autoCloseTimer;

    float theTime;

	AudioSource soundManager;
	public AudioClip doorOpen, doorClose;
	public AudioClip doorRattle;


	// Use this for initialization
	void Start () {
		soundManager = GameObject.Find ("SoundManager").GetComponent<AudioSource>();
        flashlight = GameObject.Find("Flashlight");
        hinge = transform.GetChild(0);

	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("Timer: " + timer + ". Time: " + Time.time + ". theTime: " + theTime);
        if (timer)
        {
            if (Time.time - theTime >= autoCloseTimer)
            {
                open = !open;
                closing = true;
                timer = false;
                soundManager.PlayOneShot(doorClose, 1.0f);
            }
        }
        
        if (open && transform.localEulerAngles.y >= 90f)
        {
            if (!timer)
            {
                theTime = Time.time;
                timer = true;
            }
            opening = false;
        }
        else if (!open && transform.localEulerAngles.y >= 350f)
        {
            closing = false;
        }
        //Debug.Log(transform.localEulerAngles.y + " " + closing);
        if (Input.GetButtonDown("Use"))
        {
            if (!transform.parent.GetComponent<DoorScript>().locked)
            {
                //Debug.Log ("press");

                Ray rayToPlayer = new Ray(flashlight.transform.position, flashlight.transform.forward);
                RaycastHit rayHitToPlayer = new RaycastHit();

                if (Physics.Raycast(rayToPlayer, out rayHitToPlayer))
                {
                    //Debug.Log(rayHitToPlayer.collider.name);
                    if (rayHitToPlayer.collider.name == "DoorHinge" && Vector3.Distance(flashlight.transform.position, transform.position) <= 3f)
                    {
                        open = !open;
                        if (open)
                        {
                            opening = true;
                            soundManager.PlayOneShot(doorOpen, 1.0f);
                        }
                        else {
                            closing = true;
                            soundManager.PlayOneShot(doorClose, 1.0f);
                        }
                    }

                }
            }
            //When kid locks and tries to open it
            else
            {
				soundManager.PlayOneShot (doorRattle, 1f);
            }
        }
        //Debug.Log(hinge.localEulerAngles.y);


       // Debug.Log(angle);

        if (open && opening) 
        {
			//add open sound

            transform.Rotate(0, speed * Time.deltaTime, 0);
        }

        if (!open && closing)
        {
            //add close sound
            transform.Rotate(0, -speed * Time.deltaTime, 0);
        }

    }
}
