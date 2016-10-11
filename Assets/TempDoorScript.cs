using UnityEngine;
using System.Collections;

public class TempDoorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        transform.FindChild("Cube").gameObject.SetActive(false);
    }

    void OnTriggerExit(Collider col)
    {
        transform.FindChild("Cube").gameObject.SetActive(true);
    }
}
