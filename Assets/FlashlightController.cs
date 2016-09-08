using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour {

    [SerializeField]
    float flashLightSpeed;

    private float inputX;
    private Animation controlAnimation;

    // Use this for initialization
    void Start() {
        GetComponent<Animator>().Stop();
        controlAnimation = GetComponent<Animation>();

    }
    // Update is called once per frame
    void Update()
    {

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
