using UnityEngine;
using System.Collections;

public class GridLocker : MonoBehaviour {

    [SerializeField]
    Transform gridBase;

    [SerializeField]
    float height;

    [SerializeField]
    Vector3 gridLocation;

    public bool locked = true;

    private float gridXPosition, gridYPosition;
    private Vector3 currentLocation;
    private GridInfo gridInfo;


	// Use this for initialization
	void Start () {
        currentLocation = CalculateGridToReal(gridLocation);
        gridInfo = gridBase.GetComponent<GridInfo>();
    }
	
	// Update is called once per frame
	void Update () {
        gridInfo = gridBase.GetComponent<GridInfo>();
        //Debug.Log(gridBase.GetComponent<GridInfo>().gridMax);

        if (CalculateGridToReal(gridLocation) != transform.position && locked)
        {
            currentLocation = CalculateGridToReal(gridLocation);
            Debug.Log(CalculateGridToReal(gridLocation));
            transform.position = currentLocation;
        }
        if (!locked)
        {
            gridLocation = CalculateRealToGridLocation(transform.position);
        }
       
	}

    public Vector3 CalculateGridToReal(Vector3 coordinates)
    {
        Vector3 location;
        gridInfo = gridBase.GetComponent<GridInfo>();
        coordinates = new Vector3(Mathf.Clamp(coordinates.x, gridInfo.gridMin, gridInfo.gridMax),
                                    coordinates.y,
                                    Mathf.Clamp(coordinates.z, gridInfo.gridMin, gridInfo.gridMax));
        location = new Vector3(coordinates.x * 20, height, coordinates.z * 20);
        return location;
    }

    public Vector3 CalculateRealToGridLocation(Vector3 position)
    {
        Vector3 coordinates;
        gridInfo = gridBase.GetComponent<GridInfo>();
        gridXPosition = Mathf.Clamp((int)(position.x / 20), gridInfo.gridMin, gridInfo.gridMax);
        gridYPosition = Mathf.Clamp((int)(position.z / 20), gridInfo.gridMin, gridInfo.gridMax);
        coordinates = new Vector3(gridXPosition, height, gridYPosition); 

        return coordinates;
    }
    
}
