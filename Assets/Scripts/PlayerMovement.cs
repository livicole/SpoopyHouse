using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    public float walkingSpeed;

    [SerializeField]
    public float turningSpeed;

    // Use this for initialization
    void Start()
    {


    }
	// Update is called once per frame
	void Update () {
        CharacterController charCont = GetComponent<CharacterController>();

        float xAxis = Input.GetAxis("HorizontalMovement");
        float yAxis = -Input.GetAxis("VerticalMovement");

        Vector3 moveVector = ((transform.forward * yAxis) + (transform.right * xAxis)).normalized * walkingSpeed;



        charCont.Move((transform.forward * walkingSpeed * yAxis) + (transform.right * walkingSpeed * xAxis));
        //transform.Rotate(new Vector3(0, turningSpeed * xAxis, 0));

        float camXAxis = Input.GetAxis("HorizontalCamera");
       
        transform.Rotate(new Vector3(0, turningSpeed * camXAxis, 0));



	}
}
