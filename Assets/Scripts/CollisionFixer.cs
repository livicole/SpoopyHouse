using UnityEngine;
using System.Collections;

public class CollisionFixer : MonoBehaviour {

    //Variables for physical strength.
    [SerializeField]
    float weight;

    [SerializeField]
    float pushStrength;

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        Vector3 force;
        // no need to push if no rigidboy/kinematic.
        if (rb == null || rb.isKinematic) { return; }

        //Don't push down on objects.
        if (hit.moveDirection.y < -0.3)
        {
            //-0.5f is a variable for how much force is used downwards.
            //10 is a movement variable which can be modified.
            force = new Vector3(0, -0.5f, 0) * 10 * weight;
        }
        else
        {
            force = hit.controller.velocity * pushStrength;
        }

        //Use the push.
        rb.AddForceAtPosition(force, hit.point);

    }
}
