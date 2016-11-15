using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] rooms;

    public bool gameIsLive = true, connection;
    public Transform childPlayer;
    private Vector3 startingPosition;
    private Transform grid;
    private GridInfo gridInfo;

    float timer, navMeshBuildInterval = 5;
	// Use this for initialization
	void Start () {

        rooms = GameObject.FindGameObjectsWithTag("Room");

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
