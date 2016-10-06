using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{


    public List<GameObject> inventoryList = new List<GameObject>();

    int activeObjectIndex = 0;

    public Transform child;

    public Text activeItemText;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void pickUp(GameObject myObject)
    {

        inventoryList.Add(myObject);

        if (inventoryList.Count == 1)
        {
            activeItemText.text = myObject.name;
        }

        Debug.Log("Picked up " + myObject.name + " and I'm holding " + inventoryList.Count + " items. It's at position " + inventoryList.IndexOf(myObject));

       /* if (inventoryList.Count==3)
        {
            activeItemText.text = "You have 3 items";
        }*/
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
                activeItemText.text = "";
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
