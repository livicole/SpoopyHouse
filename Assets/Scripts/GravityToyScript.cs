using UnityEngine;
using System.Collections;

public class GravityToyScript : MonoBehaviour
{

    GameObject theChild;
    public bool hasSetParent = false;


    public Transform theRoom, decoration;

    // Use this for initialization
    void Start()
    {
        theChild = GameObject.Find("ChildPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (hasSetParent)
        {
            theRoom = transform.parent;
        }

        Ray rayToPlayer = new Ray(transform.position, theChild.transform.position - transform.position);
        RaycastHit rayToPlayerHit = new RaycastHit();
        //Debug.DrawRay(transform.position, theChild.transform.position - transform.position, Color.red);
        if (Physics.Raycast(rayToPlayer, out rayToPlayerHit))
        {
            if (rayToPlayerHit.collider.name == "ChildPlayer")
            {
                //Debug.Log("See!");
                GameObject[] roomDecors = GameObject.FindGameObjectsWithTag("Decor");
                foreach (GameObject decors in roomDecors)
                {
                    //Debug.Log(decors);
                    if(decors.transform.parent.parent.Equals(transform.parent))
                    {
                        decoration = decors.transform;
                        //Debug.Log("True");
                        foreach (Transform anObject in decoration)
                        {
                            //Debug.Log("Checking....");
                            if (anObject.GetComponent<ToyReceptor>() != null)
                            {
                                if (anObject.GetComponent<ToyReceptor>().fuckYou == false)
                                {
                                    anObject.GetComponent<ToyReceptor>().raycastFound();
                                }
                            }
                        }
                    }
                } 
            }
        }
    }

    void OnDestroy()
    {
        foreach (Transform anObject in decoration)
        {
            if(anObject.GetComponent<ToyReceptor>() != null)
            {
                anObject.GetComponent<ToyReceptor>().stopHovering();
                anObject.GetComponent<ToyReceptor>().ResetValues();
            }
           
        }
    }

}
