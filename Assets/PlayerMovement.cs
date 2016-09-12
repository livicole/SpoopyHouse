using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    float walkingSpeed;

    [SerializeField]
    float turningSpeed;

    // Use this for initialization
    void Start()
    {


    }
	// Update is called once per frame
	void Update () {
        CharacterController charCont = GetComponent<CharacterController>();

        float xAxis = Input.GetAxis("HorizontalMovement");
        float yAxis = -Input.GetAxis("VerticalMovement");

        charCont.Move(transform.forward * walkingSpeed * yAxis);
        transform.Rotate(new Vector3(0, turningSpeed * xAxis, 0));

        /**
        if (Input.GetKey(KeyCode.W))
        {
            charCont.Move(transform.forward * walkingSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            charCont.Move(transform.forward * -walkingSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -turningSpeed, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, turningSpeed, 0));
            //charCont.Move(transform.right * walkingSpeed);
        }**/
	}
}
