using UnityEngine;
using System.Collections;

public class GravityToyScript : MonoBehaviour
{

    GameObject theChild;


    public Transform theRoom;

    // Use this for initialization
    void Start()
    {
        theChild = GameObject.Find("ChildPlayer");
    }

    // Update is called once per frame
    void Update()
    {


        Ray rayToPlayer = new Ray(transform.position, theChild.transform.position - transform.position);
        RaycastHit rayToPlayerHit = new RaycastHit();

        if (Physics.Raycast(rayToPlayer, out rayToPlayerHit))
        {
            if (rayToPlayerHit.collider.name == "ChildPlayer")
            {
                foreach (Transform anObject in theRoom)
                {
                    if (anObject.GetComponent<ToyReceptor>().fuckYou == false)
                    {
                        anObject.GetComponent<ToyReceptor>().raycastFound();
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        foreach (Transform anObject in theRoom)
        {
            anObject.GetComponent<ToyReceptor>().stopHovering();
        }
    }

}
