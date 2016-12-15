using UnityEngine;
using System.Collections;

public class ShakeTimer : MonoBehaviour {

    float shakeIntensity = 0.001f;
    float Timer = 0f;

    Vector3 position;

    void Start()
    {
        position = transform.position;
    }

	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime;

        shakeIntensity = (int)Timer * 0.001f;

        if((int)Timer % 2 > 0)
        {
            transform.position = shakeIntensity * Random.insideUnitSphere + position;
        }
       

	}
}
