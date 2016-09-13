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



        charCont.Move((transform.forward * walkingSpeed * yAxis) + (transform.right * walkingSpeed * xAxis));
        //transform.Rotate(new Vector3(0, turningSpeed * xAxis, 0));

        float camXAxis = Input.GetAxis("HorizontalCamera");
        float camYAxis = Input.GetAxis("VerticalCamera");
        transform.Rotate(new Vector3(turningSpeed * camYAxis, turningSpeed * camXAxis, 0));



	}
}
