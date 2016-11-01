using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool gameIsLive = true;

	// Use this for initialization
	void Start () {
	
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
}
