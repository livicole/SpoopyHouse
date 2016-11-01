using UnityEngine;
using System.Collections;

public class ItemInfo : MonoBehaviour {

    //Stuff for gravity thing
    public bool preventInitialLerp = false;
    public bool shouldIFloat = false;
    public bool startSinWave = false;
    public bool startTimer = false;
    float timer = 0;

    public Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (startTimer)
        {
            timer = timer + 0.01f;
        }
	
	}
}
