using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FearInfo : MonoBehaviour {

    public float fearLevel = 0;
    public Text death;

    public Scrollbar fearMeter;

    bool isAlive = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(fearMeter.size >= 1)
        {
            isAlive = false;
            death.text = "YOU DIED";
            Debug.Log("You dead.");
        }

        fearMeter.size += fearLevel / 10000;
	}
}
