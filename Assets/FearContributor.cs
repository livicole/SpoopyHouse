using UnityEngine;
using System.Collections;

public class FearContributor : MonoBehaviour {

    [SerializeField]
    Transform child;

    private float fearLevel, timer;
    [SerializeField]
    float fearGain;
    [SerializeField]
    float fearGainRate;
    [SerializeField]
    float shakeThreshold;
    [SerializeField]
    float shakeSpeed;

	// Use this for initialization
	void Start () {
        child = GameObject.Find("ChildPlayer").transform;
        timer = 0;
        fearLevel = 0;
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
            child.GetComponent<FearInfo>().fearLevel += fearGain;
            fearLevel += fearGain;
        }

        if(fearLevel > shakeThreshold)
        {
            //Slides the item around as if its being haunted.
            transform.position = new Vector3(Mathf.Sin(Time.time * shakeSpeed), transform.position.y, transform.position.z);
        }

        
	}
}
