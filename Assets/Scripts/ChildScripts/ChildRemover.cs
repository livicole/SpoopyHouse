﻿using UnityEngine;
using System.Collections;

public class ChildRemover : MonoBehaviour {

    Transform flashlight;

    [SerializeField]
    float detectDistance;

    [SerializeField]
    float destroyCooldown;

    private float destroyTimer;
    private bool coolingdown;
    public bool holdingLamp = false;
    [SerializeField]
    Transform lamp;

	public AudioSource soundManager;
	public AudioClip chimeSound;

    public Transform gameManager;

    //public Transform inventoryObject;


	// Use this for initialization
	void Start () {
        flashlight = GameObject.Find("Flashlight").transform;
        gameManager = GameObject.Find("GameManager").transform;
	}
	
	// Update is called once per frame
	void Update () {

        if (coolingdown)
        {
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= destroyCooldown)
            {
                destroyTimer = 0;
                coolingdown = false;
            }
        }

        if (GameObject.Find("GameManager").GetComponent<GameManager>().gameIsLive)
        {
            
            if (Input.GetButtonDown("Use"))
            {
                gameManager.GetComponent<GameManager>().CheckToTrigger();
            }

            if (Input.GetButton("Use"))
            {
                if (!coolingdown)
                {
                    
                    //Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y - heightDifference, transform.position.z);
                    Vector3 rayPosition = flashlight.transform.position;
                    Vector3 rayDirection = flashlight.transform.forward;
                    Ray forwardRay = new Ray(rayPosition, rayDirection);
                    RaycastHit forwardRayHit = new RaycastHit();
                    Debug.DrawRay(rayPosition, rayDirection * detectDistance, Color.red);

                    if (Physics.Raycast(forwardRay, out forwardRayHit, detectDistance))
                    {
                        //Debug.Log(forwardRayHit.collider.gameObject.layer);
                        if (forwardRayHit.collider.gameObject.tag == "Lamp")
                        {
                            Destroy(forwardRayHit.collider.gameObject);
                            holdingLamp = true;
                        }
//                        else if(forwardRayHit.collider.gameObject.tag == "Battery")
//                        {
//                            Destroy(forwardRayHit.collider.gameObject);
//                            GameObject.Find("Flashlight").GetComponent<FlashlightController>().batteryCount++;
//                        }
                        else if (forwardRayHit.collider.gameObject.tag == "Item")
                        {
                            //inventoryObject.GetComponent<InventoryScript>().pickUp(forwardRayHit.collider.gameObject);
                            NewInventoryScript invScript = this.gameObject.GetComponent<NewInventoryScript>();
                            invScript.itemsCollected++;
							soundManager.PlayOneShot (chimeSound, 1f);
							if (forwardRayHit.collider.gameObject.name == "NauthizItem(Clone)") {
								invScript.nauthCollected = true;
							}
							if (forwardRayHit.collider.gameObject.name == "OthalaItem(Clone)") {
								invScript.othalCollected = true;
							}
							if (forwardRayHit.collider.gameObject.name == "PerthroItem(Clone)") {
								invScript.perthCollected = true;
							}
							if (forwardRayHit.collider.gameObject.name == "ThurisazItem(Clone)") {
								invScript.thuriCollected = true;
							}
							if (forwardRayHit.collider.gameObject.name == "AlgizItem(Clone)") {
								invScript.algizCollected = true;
							}
							forwardRayHit.collider.gameObject.SetActive(false);
                        }

                    }
                }
                else
                {
                    Debug.Log("On Cooldown");
                }
            }

            if (Input.GetButtonDown("Drop") && holdingLamp)
            {
                holdingLamp = false;
                Instantiate(lamp, transform.position + transform.forward * 2, Quaternion.identity);
            }
        }

        //if (Input.GetButtonDown())
	}

//        if (Input.GetButtonDown("Drop"))
//        {
//            inventoryObject.GetComponent<InventoryScript>().dropItem();
//        }
//
//        if (Input.GetButtonDown("Cycle"))
//        {
//            inventoryObject.GetComponent<InventoryScript>().cycleActiveItem();
//        }
//    }
}
