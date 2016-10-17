using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GoalScript : MonoBehaviour {


    public Text winText;
    

	// Use this for initialization
	void Start () {
        winText = GameObject.Find("WinText").GetComponent<Text>();
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "ChildPlayer")
        {
			if (col.gameObject.GetComponentInChildren<NewInventoryScript>().itemsCollected == 5)
            {
                //Debug.Log("He won!");
                winText.text = "You Win!";
            }
        }
    }
}
