using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    [SerializeField]
    float speed;

    [SerializeField]
    Transform spawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float inputX = Input.GetAxis("HorizontalMovement2");
        float inputY = Input.GetAxis("VerticalMovement2");

        GetComponent<CharacterController>().Move(new Vector3(inputX, 0, -inputY) * speed);

        if (Input.GetButton("Spawn"))
        {
            Ray verticalRay = new Ray(transform.position, Vector3.down);
            RaycastHit verticalRayHit = new RaycastHit();

            if (Physics.Raycast(verticalRay, out verticalRayHit, 100f))
            {
                if (verticalRayHit.collider.gameObject.layer.Equals("Floor"))
                {
                    Instantiate(spawn, verticalRayHit.point, Quaternion.identity);
                }
            }
        }
	}
}
