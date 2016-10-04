using UnityEngine;
using System.Collections;

public class ToyReceptor : MonoBehaviour
{
    public bool fuckYou = false;
    public bool startTimer = false;
    bool shouldIHover = false;
    bool preventInitialLerp = false;
    bool startSinWave = false;
    float timer = 1;
    public float sinRange;
    public float timerIncrement;
    public float hoverHeight;
    public float startSinHeight;
    float randomDiff;
    public float lerpT;

    float randomNumber;


    Vector3 totalHeight;
    Vector3 originalPosition;
    Vector3 upWithDiff;


    // Use this for initialization
    void Start()
    {
        //randomDiff = Random.Range(0, 3f);
        //upWithDiff = new Vector3 (0, 1+randomDiff, 0);
        randomNumber = Random.Range(0, 4);
        startSinHeight = 1.9f + randomNumber;
        hoverHeight = 2f + randomNumber;
        sinRange = 100f - (20 * randomNumber);
        timerIncrement = 0.02f + (randomNumber / 100f);

    }

    // Update is called once per frame
    void Update()
    {

        if (startTimer)
        {
            timer = timer + timerIncrement;
        }


        if (shouldIHover)
        {
            Debug.Log("shouldihover");
          //transform.position = Vector3.Lerp(transform.position, originalPosition + upWithDiff * hoverHeight, lerpT);
            transform.position = Vector3.Lerp(transform.position, originalPosition + Vector3.up * hoverHeight, lerpT);
        }

        //if (transform.position.y > originalPosition.y + startSinHeight + randomDiff)
        if (transform.position.y > originalPosition.y + startSinHeight)
        {
            Debug.Log("transform.position.y");

            preventInitialLerp = true;
            shouldIHover = false;
            startSinWave = true;
            Debug.Log(transform.position);
        }

        if (startSinWave)
        {
            Debug.Log("startsinwave");
            transform.position += new Vector3(0, (Mathf.Sin(timer)) / sinRange, 0);
        }

    }

    public void raycastFound()
    {
        startTimer = true;
        Debug.Log("raycastfound");
        fuckYou = true;
        GetComponent<Rigidbody>().useGravity = false;
        originalPosition = transform.position;
        Debug.Log(transform.position);
        shouldIHover = true;
        
        //totalHeight = originalPosition + upWithDiff * hoverHeight;
        //float yDistance = Mathf.Abs( totalHeight.y - originalPosition.y);
        //Debug.Log("y distance: " + yDistance);
        //lerpT = (yDistance -2) * -.003f + .03f;

    }


    public void stopHovering()
    {
        startTimer = false;
        shouldIHover = false;
        startSinWave = false;
        preventInitialLerp = false;
        timer = 0;
        GetComponent<Rigidbody>().useGravity = true;    
    }

}
