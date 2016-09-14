using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour {

    [SerializeField]
    float flashLightSpeed;

    [SerializeField]
    float minimumRange;

    [SerializeField]
    float maximumRange;

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

    // Use this for initialization
    void Start() {

        controlAnimator = GetComponent<Animator>();
        flashlight = GetComponent<Light>();

        angleChange = (maximumAngle - minimumAngle) / timeFromMinToMax;
        rangeChange = (maximumRange - minimumRange) / timeFromMinToMax;
        Debug.Log("Range :" + rangeChange);
 
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
        
        //Increased range and narrower beam.
        if (Input.GetButton("LeftBumper"))
        {
            if(flashlight.range <= maximumRange)
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
