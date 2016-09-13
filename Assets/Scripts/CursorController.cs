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
            Debug.Log("Detected");
            Ray verticalRay = new Ray(transform.position, Vector3.down * 100f);
            Debug.DrawRay(transform.position, Vector3.down * 100f, Color.red);
            RaycastHit verticalRayHit = new RaycastHit();

            if (Physics.Raycast(verticalRay, out verticalRayHit, 100f))
            {
                if (verticalRayHit.collider.gameObject.layer == 10)
                {
                    Instantiate(spawn, verticalRayHit.point, Quaternion.identity);
                }
            }
        }
	}
}
