﻿using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour {

    [SerializeField]
    float flashLightSpeed;

    private float inputX;
    private Animator controlAnimator;

    // Use this for initialization
    void Start() {

        controlAnimator = GetComponent<Animator>();
        //controlAnimator.speed = 0.0f;

    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Trigger: " + Input.GetAxis("LeftTrigger"));
        float left = -Input.GetAxis("LeftTrigger"); //-1 -> 0
        float right = Input.GetAxis("RightTrigger"); //0 -> 1
        float combination = left + right;
        //inputX = Input.GetAxis("LeftTrigger");
        inputX = (combination + 1) / 2;
        if(inputX == 1)
        {
            inputX = 0.99f;
        }

        //Debug.Log("Input X: " + (double)(inputX));
        controlAnimator.SetTime((double)(inputX));


        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Animator>().StartPlayback();
            //GetComponent<Animator>().Play("ControlAnimation", 0);
        }

        /**
            controlAnimation.GetComponent<Time>(). = inputX;
            controlAnimation.speed = 0.0f;
            */
        /**
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(new Vector3(-flashLightSpeed, 0, 0));
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(flashLightSpeed, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, -flashLightSpeed, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, flashLightSpeed, 0));
        }*/
    }

}
