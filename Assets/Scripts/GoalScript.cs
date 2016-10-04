﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GoalScript : MonoBehaviour {


    public Text winText;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
      


	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "ChildPlayer")
        {
            //Debug.Log("Its the kid!");
            if (col.gameObject.GetComponentInChildren<InventoryScript>().inventoryList.Count == 5)
            {
                //Debug.Log("He won!");
                winText.text = "You Win!";
            }
        }
    }
}
