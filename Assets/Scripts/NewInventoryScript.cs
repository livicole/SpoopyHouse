using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewInventoryScript : MonoBehaviour {

	public Image keyItem1, keyItem2, keyItem3, keyItem4, keyItem5;
	public int itemsCollected;

	// Use this for initialization
	void Start () {
		itemsCollected = 0;
		keyItem1.color = Color.white;
		keyItem2.color = Color.white;
		keyItem3.color = Color.white;
		keyItem4.color = Color.white;
		keyItem5.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		if (itemsCollected == 1) {
			keyItem1.color = Color.yellow;
		}
		if (itemsCollected == 2) {
			keyItem2.color = Color.yellow;
		}
		if (itemsCollected == 3) {
			keyItem3.color = Color.yellow;
		}
		if (itemsCollected == 4) {
			keyItem4.color = Color.yellow;
		}
		if (itemsCollected == 5) {
			keyItem5.color = Color.yellow;
		}
	}
}
