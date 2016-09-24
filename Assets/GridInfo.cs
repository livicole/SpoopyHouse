using UnityEngine;
using System.Collections;

public class GridInfo : MonoBehaviour {

    [SerializeField]
    float gridSize;

    [SerializeField]
    float blockLength;

    public float gridMax, gridMin;

	// Use this for initialization
	void Start () {
        float length = (gridSize + 1) * blockLength;
	    transform.localScale = new Vector3 (length, 1, length);
        transform.position = new Vector3(length / 2, 0, length / 2);
        gridMax = gridSize; gridMin = 0;
	}
	
}
