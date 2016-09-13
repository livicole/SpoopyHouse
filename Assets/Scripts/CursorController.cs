using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    [SerializeField]
    float speed;

    Transform child;

    [SerializeField]
    Transform spawn;

	// Use this for initialization
	void Start () {
        child = GameObject.Find("ChildPlayer").transform;
	}
	
	// Update is called once per frame
	void Update () {
        float inputX = Input.GetAxis("HorizontalMovement2");
        float inputY = Input.GetAxis("VerticalMovement2");

        GetComponent<CharacterController>().Move(new Vector3(inputX, 0, -inputY) * speed);

        if (Input.GetButtonDown("Spawn"))
        {
            Debug.Log("Detected");
            Ray verticalRay = new Ray(transform.position, Vector3.down * 100f);
            Debug.DrawRay(transform.position, Vector3.down * 100f, Color.red);
            RaycastHit verticalRayHit = new RaycastHit();

            if (Physics.Raycast(verticalRay, out verticalRayHit, 100f))
            {
                if (verticalRayHit.collider.gameObject.layer == 10)
                {
                    Vector3 spawnPosition = new Vector3(verticalRayHit.point.x, 1, verticalRayHit.point.z);
                    Instantiate(spawn, spawnPosition, Quaternion.identity);
                    child.GetComponent<FearInfo>().fearLevel += 1;
                }
            }
        }
	}
}
