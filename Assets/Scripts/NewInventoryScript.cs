using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewInventoryScript : MonoBehaviour {

	public Image keyItem1, keyItem2, keyItem3, keyItem4, keyItem5;
	public int itemsCollected;
    public Color emptyColor, collectedColor;

	// Use this for initialization
	void Start () {
		itemsCollected = 0;
        keyItem1.color = emptyColor;
        keyItem2.color = emptyColor;
		keyItem3.color = emptyColor;
		keyItem4.color = emptyColor;
		keyItem5.color = emptyColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (itemsCollected == 1) {
            keyItem1.color = collectedColor;
		}
		if (itemsCollected == 2) {
			keyItem2.color = collectedColor;
		}
		if (itemsCollected == 3) {
			keyItem3.color = collectedColor;
		}
		if (itemsCollected == 4) {
			keyItem4.color = collectedColor;
		}
		if (itemsCollected == 5) {
			keyItem5.color = collectedColor;
		}
	}
}
