using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FearMeter : MonoBehaviour {

    public Scrollbar fearMeter;

    public float numberFearAdded = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.O))
        {
            numberFearAdded--;

        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            numberFearAdded++;

        }
        fearMeter.size -= numberFearAdded/5000;
        if (fearMeter.size == 0)
        {
            Debug.Log("AHH IM SCARED");
        }

	}
}
