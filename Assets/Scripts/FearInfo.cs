using UnityEngine;
using System.Collections;

public class FearInfo : MonoBehaviour {

    public float fearLevel = 0;

    bool isAlive = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(fearLevel > 100)
        {
            isAlive = false;
        }


	}
}
