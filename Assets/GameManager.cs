﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {


    //List<GameObject> rooms = new List<GameObject>();
    GameObject[] rooms;
    bool allRoomsConnected = true;


    public bool gameIsLive = true;
    public Transform childPlayer;
    private Vector3 startingPosition;
    private Transform grid;
    private GridInfo gridInfo;
	// Use this for initialization
	void Start () {
        childPlayer = GameObject.Find("ChildPlayer").transform;
        grid = GameObject.Find("GridBase").GetComponent<GridInfo>().transform;
        gridInfo = grid.GetComponent<GridInfo>();
        startingPosition = new Vector3((gridInfo.gridSize / 2 + 0.5f) * gridInfo.blockLength, 1, (gridInfo.gridSize / 2 + 0.5f) * gridInfo.blockLength);
        Debug.Log("Starting position: " + startingPosition);
        childPlayer.position = startingPosition;

        rooms = GameObject.FindGameObjectsWithTag("Room");

	}
	
	// Update is called once per frame
	void Update () {

        if (!gameIsLive)
        {
            if(Input.GetButtonDown("Ghost Button A") || Input.GetButtonDown("Use"))
            {
                SceneManager.LoadScene("0");
            }
        }



	}

    public bool CheckRooms()
    {
        foreach (GameObject room in rooms)
        {
            if (!room.GetComponent<GridLocker>().amIConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
