using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewInventoryScript : MonoBehaviour {

	public Image keyItem1, keyItem2, keyItem3, keyItem4, keyItem5;
	public float itemsCollected;
    public Color emptyColor, collectedColor;
	public bool nauthCollected, algizCollected, othalCollected, perthCollected, thuriCollected;

	// Use this for initialization
	void Start () {
        keyItem1.color = emptyColor;
        keyItem2.color = emptyColor;
		keyItem3.color = emptyColor;
		keyItem4.color = emptyColor;
		keyItem5.color = emptyColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (algizCollected == true) {
            keyItem1.color = collectedColor;
		}
		if (perthCollected == true) {
			keyItem2.color = collectedColor;
		}
		if (nauthCollected == true) {
			keyItem3.color = collectedColor;
		}
		if (thuriCollected == true) {
			keyItem4.color = collectedColor;
		}
		if (othalCollected == true) {
			keyItem5.color = collectedColor;
		}
	}
}
