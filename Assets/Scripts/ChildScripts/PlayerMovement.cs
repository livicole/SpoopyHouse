using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    public float walkingSpeed;

    [SerializeField]
    public float sensitivity;

    public bool invert = false;

	public AudioSource walkingSound;

    public float yRotation;
    private float currentYRotation, yRotationV, lookSmoothDamp;

    // Use this for initialization
    void Start()
    {
        Physics.IgnoreLayerCollision(17, 14);
        yRotation = 0;
        yRotationV = 1f;
        lookSmoothDamp = 0.05f;

    }
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().gameIsLive)
        {


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
            
            yRotation += camXAxis * sensitivity;
            currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);

            Vector3 rotation = new Vector3(transform.localEulerAngles.x, currentYRotation, 0);
            transform.localRotation = Quaternion.Euler(rotation);


            if (charCont.velocity == Vector3.zero)
            {
                walkingSound.enabled = false;
                walkingSound.loop = false;
            }
            else {
                walkingSound.enabled = true;
                walkingSound.loop = true;
            }
        }

	}
}
