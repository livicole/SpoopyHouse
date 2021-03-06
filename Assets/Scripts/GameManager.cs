﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] rooms;

    public GameObject[] OpenableDrawers;
    public GameObject[] OpenableRightDoors;
    public GameObject[] OpenableLeftDoors;

    public GameObject[] runes = new GameObject[5];

    bool[] randomNumBools;

    public bool gameIsLive = true, connection;
    public Transform childPlayer;
    private Vector3 startingPosition;
    private Transform grid;
    private GridInfo gridInfo;

    float timer, navMeshBuildInterval = 5;
	// Use this for initialization
	void Start () {
        

        rooms = GameObject.FindGameObjectsWithTag("Room");
        OpenableDrawers = GameObject.FindGameObjectsWithTag("Drawer");
        OpenableRightDoors = GameObject.FindGameObjectsWithTag("leftDoor");
        OpenableLeftDoors = GameObject.FindGameObjectsWithTag("rightDoor");
        randomNumBools = new bool[rooms.Length];

        //pick rooms to instantiate key items in
        int roomsFound = 0;

        //until 5 rooms are found
        while (roomsFound < 5)
        {
            //get a random number
            int randomNumber = Random.Range(0, 12);

            //ignore rooms that dont have key item spawns
            if (rooms[randomNumber].name != "1x1Room4Doors" && rooms[randomNumber].name != "PlusRoom")
            {

                //if we haven't used this room already
                if (!randomNumBools[randomNumber])
                {
                    foreach (Transform findChild in rooms[randomNumber].transform.GetChild(0))
                    {
                        if (findChild.name == "KeyItemSpawns")
                        {
                            int useSpawnNumber = Random.Range(0, findChild.childCount);
                            GameObject newKeyItem = Instantiate(runes[roomsFound], findChild.GetChild(useSpawnNumber).position, Quaternion.identity) as GameObject;

                            if (findChild.GetChild(useSpawnNumber).GetComponent<ItemInADrawer>() != null)
                            {
                                newKeyItem.transform.SetParent(findChild.GetChild(useSpawnNumber).GetComponent<ItemInADrawer>().drawer);
                            }
                            else
                            {
                                newKeyItem.transform.SetParent(findChild.GetChild(useSpawnNumber));
                            }
                            
                        }
                    }
                    randomNumBools[randomNumber] = true;
                    roomsFound++;
                }
            }
        }
        //Debug.Log("IM FREE");

        childPlayer = GameObject.Find("ChildPlayer").transform;
        grid = GameObject.Find("GridBase").GetComponent<GridInfo>().transform;
        gridInfo = grid.GetComponent<GridInfo>();
        startingPosition = new Vector3((gridInfo.gridSize / 2 + 0.5f) * gridInfo.blockLength, 1, (gridInfo.gridSize / 2 + 0.5f) * gridInfo.blockLength);
        //Debug.Log("Starting position: " + startingPosition);
        childPlayer.position = startingPosition;
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
        else
        {
            timer += Time.deltaTime;
            if(timer > navMeshBuildInterval)
            {
                timer = 0;
                //UnityEditor.NavMeshBuilder.BuildNavMesh();

            }
        }
	}

    
    public void CheckToTrigger()
    {
        for(int i = 0; i < OpenableDrawers.Length; i++)
        {
            OpenableDrawers[i].GetComponent<Dresser>().CheckAndTrigger();
        }

        for(int i =0; i < OpenableLeftDoors.Length; i++)
        {
            OpenableLeftDoors[i].GetComponent<Closets>().CheckAndTrigger();
        }

        for(int i = 0; i < OpenableRightDoors.Length; i++)
        {
            OpenableRightDoors[i].GetComponent<Closets>().CheckAndTrigger();
        }
    }
    
    /**
    public bool AreAllRoomsConnected()
    {
        foreach (GameObject room in rooms)
        {
            Debug.Log(room.name);
            if (!room.GetComponent<GridLocker>().amIConnected)
            {
                connection = false;
                return false;
            }

        }
        connection = true;
        return true;
    }**/
}
