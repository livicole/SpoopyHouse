    using UnityEngine;
using System.Collections;

public class ControlInvertterController : MonoBehaviour {

    Transform player;
    float distance;

    [SerializeField]
    float radiusOfEffect;

    [SerializeField]
    float maxTime;

    private float playerFov;
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

    // Use this for initialization
    void Start () {
        player = GameObject.Find("ChildPlayer").transform;
        originalPos = transform.position;
        originalPos.y = 2;
    }
	
	// Update is called once per frame
	void Update () {
        distance = Mathf.Abs(Vector3.Distance(transform.position, player.position));
        transform.position = originalPos;

        if (stareTimer >= maxTime)
        {
            player.GetComponent<CursorController>().inverterCount--;
            Destroy(gameObject);
        }
        else if (stareTimer >= shakeThreshold)
        {
            shake = true;
        }

        if (shake)
        {
            shakeSpeed = originalShakeSpeed + shakeGain * (stareTimer / shakeGainRate);
            //Debug.Log("Shake speed: " + shakeSpeed);
            Vector2 randomValues = Random.insideUnitCircle;
            transform.position = new Vector3(originalPos.x + (randomValues.x * shakeSpeed),
                originalPos.y, originalPos.z + (randomValues.y * shakeSpeed));
        }

        if (distance < radiusOfEffect - 1)
        {
            player.GetComponent<PlayerMovement>().invert = true;
            Debug.Log("Inverted!");
        }
        else if(distance > radiusOfEffect)
        {
            player.GetComponent<PlayerMovement>().invert = false;
            Debug.Log("Uninvert");
        }



	}
}
