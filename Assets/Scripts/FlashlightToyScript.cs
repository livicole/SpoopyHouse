﻿using UnityEngine;
using System.Collections;

public class FlashlightToyScript : MonoBehaviour {

    GameObject theChild;
    GameObject theFlashlight;
    float theDistance;

	// Use this for initialization
	void Start () {

        theFlashlight = GameObject.Find("Flashlight");
        theChild = GameObject.Find("ChildPlayer");

	}

    // Update is called once per frame
    void Update()
    {
        theDistance = Mathf.Clamp(Vector3.Distance(theChild.transform.position, transform.position), 0, 18f);
        Ray rayToPlayer = new Ray(transform.position, theChild.transform.position - transform.position);
        RaycastHit rayToPlayerHit = new RaycastHit();

        if (Physics.Raycast(rayToPlayer, out rayToPlayerHit))
        {
            if (rayToPlayerHit.collider.name == "ChildPlayer")
            {
                theFlashlight.GetComponent<Light>().intensity = (theDistance / 10);
                
            }
        }

    }

    void OnDestroy()
    {
        theFlashlight.GetComponent<Light>().intensity = 1.8f;

    }

}