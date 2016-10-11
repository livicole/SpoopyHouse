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
    public float sinRange = 100f;
    public float timerIncrement = .02f;
    public float hoverHeight = 2f;
    public float startSinHeight = 1.9f;
    float randomDiff;
    public float lerpT = .02f;

    float randomNumber;


    Vector3 totalHeight;
    Vector3 originalPosition;
    Vector3 upWithDiff;


    // Use this for initialization
    void Start()
    {
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
            transform.position = Vector3.Lerp(transform.position, originalPosition + Vector3.up * hoverHeight, lerpT);
        }
        
        if (transform.position.y > originalPosition.y + startSinHeight)
        {
            preventInitialLerp = true;
            shouldIHover = false;
            startSinWave = true;
            //Debug.Log(transform.position);
        }

        if (startSinWave)
        {
            transform.position += new Vector3(0, (Mathf.Sin(timer)) / sinRange, 0);
        }

    }

    public void raycastFound()
    {
        startTimer = true;
        fuckYou = true;
        GetComponent<Rigidbody>().useGravity = false;
        originalPosition = transform.position;
        Debug.Log(transform.position);
        shouldIHover = true;
        
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
