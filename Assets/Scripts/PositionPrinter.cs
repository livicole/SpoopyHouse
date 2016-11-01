using UnityEngine;
using System.Collections;

public class PositionPrinter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(transform.position);
        Debug.Log(name);
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(transform.position);
	}
}
