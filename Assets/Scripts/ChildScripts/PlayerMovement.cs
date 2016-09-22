﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    float walkingSpeed;

    [SerializeField]
    float turningSpeed;

    public bool invert = false;

    // Use this for initialization
    void Start()
    {


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
        

        Vector3 moveVector = ((transform.forward * yAxis) + (transform.right * xAxis)).normalized * walkingSpeed;
        charCont.Move(moveVector);

      

        //transform.Rotate(new Vector3(0, turningSpeed * xAxis, 0));

        float camXAxis = Input.GetAxis("HorizontalCamera");
       
        transform.Rotate(new Vector3(0, turningSpeed * camXAxis, 0));



	}
}
