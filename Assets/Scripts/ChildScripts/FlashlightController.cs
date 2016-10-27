﻿using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour {

    /*
    [SerializeField]
    float flashLightSpeed;
    **/
    [SerializeField]
    float minimumRange;

    [SerializeField]
    public float maximumRange;

    [SerializeField]
    float minimumAngle;

    [SerializeField]
    float maximumAngle;

    [SerializeField]
    float timeFromMinToMax;

    private float inputX;
	private Animator controlAnimator;
    private Light flashlight;
    private float angleChange, rangeChange;
    [SerializeField]
    private bool isRightTriggerUsed, firing = false, afterFire = false, recharging = false;

    private float preFireRange, preFireAngle, preFireIntensity;
    public float fireRange, fireAngle, fireIntensity, afterFireRange, afterFireAngle, afterFireIntensity;
    public float fireTime, dechargeTime, rechargeTime;
    public float batteryCount = 0;
    private float changeRate, currentLerp;

    // Use this for initialization
    void Start() {

        controlAnimator = GetComponent<Animator>();
        flashlight = GetComponent<Light>();

        angleChange = (maximumAngle - minimumAngle) / timeFromMinToMax;
        rangeChange = (maximumRange - minimumRange) / timeFromMinToMax;
       
        //Debug.Log("Range :" + rangeChange);
 
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Trigger: " + Input.GetAxis("LeftTrigger"));
        /**
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
        }**/


        //"Fire" the flashlight on right trigger
        if (Input.GetAxisRaw("RightTrigger") != 0)
        {
            if (!isRightTriggerUsed && !firing && !afterFire && !recharging && batteryCount > 0)
            {
                if (Input.GetAxisRaw("RightTrigger") > 0)
                {
                    firing = true;
                    currentLerp = 0;
                    preFireAngle = flashlight.spotAngle;
                    preFireIntensity = flashlight.intensity;
                    preFireRange = flashlight.range;
                    changeRate = 1.0f / fireTime;
                    batteryCount--;
                }
                isRightTriggerUsed = true;
            }
        }
        if (Input.GetAxisRaw("RightTrigger") == 0)
        {
            isRightTriggerUsed = false;
        }
        Debug.Log(currentLerp);

        //"fire" on firing being true, then set false when done.
        if (firing)
        {
            flashlight.range = Mathf.Lerp(preFireRange, fireRange, currentLerp);
            flashlight.intensity = Mathf.Lerp(preFireIntensity, fireIntensity, currentLerp);
            flashlight.spotAngle = Mathf.Lerp(preFireAngle, fireAngle, currentLerp);
            if(currentLerp >= 1.0)
            {
                firing = false;
                afterFire = true;
                currentLerp = 0;
                changeRate = 1.0f / dechargeTime;
                return;
            }
            currentLerp += changeRate * Time.deltaTime;
        }

        if (afterFire)
        {
            flashlight.range = Mathf.Lerp(fireRange, afterFireRange, currentLerp);
            flashlight.intensity = Mathf.Lerp(fireIntensity, afterFireIntensity, currentLerp);
            flashlight.spotAngle = Mathf.Lerp(fireAngle, afterFireAngle, currentLerp);
            if (currentLerp >= 1.0)
            {
                afterFire = false;
                recharging = true;
                changeRate = 1.0f / rechargeTime;
                currentLerp = 0;
                return;
            }
            currentLerp += changeRate * Time.deltaTime;
        }

        if (recharging)
        {
            flashlight.range = Mathf.Lerp(afterFireRange, preFireRange, currentLerp);
            flashlight.intensity = Mathf.Lerp(afterFireIntensity, preFireIntensity, currentLerp);
            flashlight.spotAngle = Mathf.Lerp(afterFireAngle, preFireAngle, currentLerp);
            if(currentLerp >= 1.0)
            {
                recharging = false;
            }
            currentLerp += changeRate * Time.deltaTime;
        }



        //When not firing you can tighten and widen the flashlight
        if (!firing)
        {
            //Increased range and narrower beam.
            if (Input.GetButton("LeftBumper"))
            {
                if (flashlight.range <= maximumRange)
                {
                    flashlight.range += rangeChange * Time.deltaTime;
                    flashlight.spotAngle -= angleChange * Time.deltaTime;
                }
            }

            //Decreased range and wider beam.
            if (Input.GetButton("RightBumper"))
            {
                if (flashlight.range >= minimumRange)
                {
                    flashlight.range -= rangeChange * Time.deltaTime;
                    flashlight.spotAngle += angleChange * Time.deltaTime;
                }
            }
        }
      
    }

}
