using UnityEngine;
using System.Collections;

public class FearContributor : MonoBehaviour {

    //Pointer to the child player.
    [SerializeField]
    Transform child;

    private float fearLevel, timer;

    //How much fear it gains per gain rate.
    [SerializeField]
    float fearGain;

    //How fast it gains more fear.
    [SerializeField]
    float fearGainRate;

    //How much fear until it shakes.
    [SerializeField]
    float shakeThreshold;

    //How much it shakes. Recommend below 1.0.
    [SerializeField]
    float shakeSpeed;

    //How much more shake it gets per gain rate.
    [SerializeField]
    float shakeGain;

    //How fast it gains more shake.
    [SerializeField]
    float shakeGainRate;

    private Vector3 originalPos;
    private float shakeTimer = 0;
    private bool shake;

	// Use this for initialization
	void Start () {
        child = GameObject.Find("ChildPlayer").transform;
        timer = 0;
        fearLevel = 0;
        originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if(timer < fearGainRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            //child.GetComponent<FearInfo>().fearLevel += fearGain;
            fearLevel += fearGain;
        }

        if(fearLevel > shakeThreshold)
        {
            shake = true;
        }

        if(shake)
        {
            if(shakeTimer < shakeGainRate)
            {
                shakeTimer += Time.deltaTime;
            }
            else
            {
                shakeTimer = 0;
                shakeSpeed += shakeGain;
            }

            Vector2 randomValues = Random.insideUnitCircle;
            transform.position = new Vector3(originalPos.x + (randomValues.x * shakeSpeed), 
                originalPos.y, originalPos.z + (randomValues.y * shakeSpeed));
        }

        
	}
}
