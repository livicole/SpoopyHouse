using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    public float walkingSpeed;

    [SerializeField]
    public float turningSpeed;

    public bool invert = false;

	public AudioSource walkingSound;

    // Use this for initialization
    void Start()
    {
        Physics.IgnoreLayerCollision(17, 14);

    }
	// Update is called once per frame
	void Update () {
        float xAxis, yAxis;
        CharacterController charCont = GetComponent<CharacterController>();
        if (!invert)
        {
            xAxis = Input.GetAxis("HorizontalMovement");
            yAxis = -Input.GetAxis("VerticalMovement");
        }
        else
        {
            xAxis = -Input.GetAxis("HorizontalMovement");
            yAxis = Input.GetAxis("VerticalMovement");
        }
        

        Vector3 moveVector = (((transform.forward * yAxis) + (transform.right * xAxis)).normalized + (-transform.up * 9.8f)) * walkingSpeed;
        charCont.Move(moveVector);

      

        //transform.Rotate(new Vector3(0, turningSpeed * xAxis, 0));

        float camXAxis = Input.GetAxis("HorizontalCamera");
       
        transform.Rotate(new Vector3(0, turningSpeed * camXAxis, 0));

		if (charCont.velocity == Vector3.zero) {
			walkingSound.enabled = false;
			walkingSound.loop = false;
		} else {
			walkingSound.enabled = true;
			walkingSound.loop = true;
		}

	}
}
