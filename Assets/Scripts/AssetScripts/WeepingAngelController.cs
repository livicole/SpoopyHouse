using UnityEngine;
using System.Collections;

public class WeepingAngelController : MonoBehaviour {

   
    private Transform player;

    [SerializeField]
    float maxTime;

    private bool chase = false;
    private float playerFov;
    private NavMeshAgent navMesh;
    private float stareTimer = 0f;

    [SerializeField]
    float shakeThreshold;

    //How much it shakes. Recommend below 1.0.
    [SerializeField]
    float shakeSpeed;

    //How much more shake it gets per gain rate.
    [SerializeField]
    float shakeGain;

    //How fast it gains more shake.
    [SerializeField]
    float shakeGainRate;

    private Vector3 originalPos;
    private float shakeTimer = 0;
    private bool shake;
    private float originalShakeSpeed;
	public float turnSpeed = 5f;


    // Use this for initialization
    void Start () {
        player = GameObject.Find("ChildPlayer").transform;
        navMesh = GetComponent<NavMeshAgent>();
        playerFov = player.FindChild("ChildCamera").GetComponent<Camera>().fieldOfView;
        Debug.Log(playerFov);
        stareTimer = 0;
        originalPos = transform.position;
        originalPos.y = -0.3801455f;
        originalShakeSpeed = shakeSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 playerLocation = player.position;
        Vector3 playerForward = player.forward;
        if (stareTimer >= maxTime)
        {
            exorciseAngel();
        }
        else if (stareTimer >= shakeThreshold)
        {
            shake = true;
        }
       

        if(shake)
        {
            shakeSpeed = originalShakeSpeed + shakeGain * (stareTimer / shakeGainRate);
            //Debug.Log("Shake speed: " + shakeSpeed);
            Vector2 randomValues = Random.insideUnitCircle;
            transform.position = new Vector3(originalPos.x + (randomValues.x * shakeSpeed),
                originalPos.y, originalPos.z + (randomValues.y * shakeSpeed));
        }


        if (chase)
        {          
			Transform frontWheel = this.gameObject.transform.GetChild (0);
			Transform backWheel1 = this.gameObject.transform.GetChild (2);
			Transform backWheel2 = this.gameObject.transform.GetChild (3);
			frontWheel.transform.Rotate (turnSpeed * Time.deltaTime, 0f, 0f);
			backWheel1.transform.Rotate (turnSpeed * Time.deltaTime, 0f, 0f);
			backWheel2.transform.Rotate (turnSpeed * Time.deltaTime, 0f, 0f);

			navMesh.SetDestination(playerLocation);

            Vector3 directionToAngel = transform.position - player.position;
            if (Vector3.Angle(playerForward, directionToAngel) < playerFov)
            {
                navMesh.Stop();
                stareTimer += Time.deltaTime;
                originalPos = transform.position;
                originalPos.y = -0.3801455f;
            }
            else
            {
                stareTimer = 0;
                shake = false;
                navMesh.Resume();
            }
        }
        else
        {
            Vector3 directionToAngel = transform.position - player.position;
            if (Vector3.Angle(playerForward, directionToAngel) < playerFov)
            {
                Ray lineOfSightRay = new Ray(player.position, directionToAngel);
                RaycastHit lineOfSightInfo = new RaycastHit();

                if(Physics.Raycast(lineOfSightRay, out lineOfSightInfo, 100f))
                {

                }
                chase = true;
            }
        }
	}

    public void exorciseAngel()
    {
        Debug.Log("Destroy this.");
        GameObject.Find("GhostCursor").GetComponent<CursorController>().angelCount--;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name == "ChildPlayer")
        {
           // GetComponent<Rigidbody>().
        }
    }
}
