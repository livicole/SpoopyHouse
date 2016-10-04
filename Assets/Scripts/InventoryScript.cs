using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{


    List<GameObject> inventoryList = new List<GameObject>();

    public bool hasOne, hasTwo, hasThree, hasFour, hasFive, hasSix, hasSeven, hasEight, hasNine, hasTen;

    public bool isOne, isTwo, isThree, isFour, isFive, isSix, isSeven, isEight, isNine, isTen;

    int activeObjectIndex = 0;

    public Transform child;

    public Text activeItemText, debugCount, debugIndex;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        debugCount.text = "Count: " + inventoryList.Count.ToString();
        debugIndex.text = "Index: " + activeObjectIndex.ToString();


        if (isOne)
        {

        }
        if (isTwo)
        {

        }
        if (isThree)
        {

        }
        if (isFour)
        {

        }
        if (isFive)
        {

        }
        if (isSix)
        {

        }
        if (isSeven)
        {

        }
        if (isEight)
        {

        }
        if (isNine)
        {

        }
        if (isTen)
        {

        }
        /////////////////////////////////////////

        if (hasOne)
        {
            //Debug.Log("I have Key1");
        }
        if (hasTwo)
        {

        }
        if (hasThree)
        {

        }
        if (hasFour)
        {

        }
        if (hasFive)
        {

        }
        if (hasSix)
        {

        }
        if (hasSeven)
        {

        }
        if (hasEight)
        {

        }
        if (hasNine)
        {

        }
        if (hasTen)
        {

        }

        //////////////
    }

    public void pickUp(GameObject myObject)
    {

        inventoryList.Add(myObject);

        if (inventoryList.Count == 1)
        {
            activeItemText.text = myObject.name;
        }

        Debug.Log("Picked up " + myObject.name + " and I'm holding " + inventoryList.Count + " items. It's at position " + inventoryList.IndexOf(myObject));

        if (inventoryList.Count==5)
        {
            activeItemText.text = "You win!";
        }
    }

    public void dropItem()
    {


        if (inventoryList.Count > 0)
        {
            inventoryList[activeObjectIndex].transform.position = transform.position;
            inventoryList[activeObjectIndex].SetActive(true);
            inventoryList.RemoveAt(activeObjectIndex);
            if (inventoryList.Count == 0)
            {
                activeObjectIndex = 0;
                activeItemText.text = "No item.";
            }
            else
            {
                activeObjectIndex--;
                if (activeObjectIndex < 0)
                {
                    activeObjectIndex = inventoryList.Count - 1;
                }
                activeItemText.text = inventoryList[activeObjectIndex].name;
            }
        }

        


    }

    public void cycleActiveItem()
    {
        if (inventoryList.Count != 0)
        {
            activeObjectIndex++;
            if (activeObjectIndex > inventoryList.Count - 1)
            {
                activeObjectIndex = 0;
            }
            activeItemText.text = inventoryList[activeObjectIndex].name;
        }
    }

}
