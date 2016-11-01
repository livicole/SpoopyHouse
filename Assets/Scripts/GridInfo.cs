using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class GridInfo : MonoBehaviour {

    [HideInInspector]
    public List<Vector4> usedGridBlocks;

    [SerializeField]
    float gridSize;

    [SerializeField]
    public float blockLength;

    public float gridMax, gridMin = 0;

    public float moveTick;

	// Use this for initialization
	void Start () {
        float length = (gridSize + 1) * blockLength;
	    transform.localScale = new Vector3 (length, 1, length);
        transform.position = new Vector3(length / 2, 0, length / 2);
        gridMax = gridSize; gridMin = 0;
	}

    public void AddBlock(Vector3 coordinate, int roomNumber)
    {
        Vector4 newVector = new Vector4(coordinate.x, coordinate.y, coordinate.z, roomNumber);
        usedGridBlocks.Add(newVector);
    }

    public void RemoveBlock(Vector3 coordinate, int roomNumber)
    {
        Vector4 newVector = new Vector4(coordinate.x, coordinate.y, coordinate.z, roomNumber);
        usedGridBlocks.Remove(newVector);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(GridInfo))]
public class GridInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = target as GridInfo;

        EditorGUILayout.LabelField("Used Coordinates from Origin");
        EditorGUILayout.LabelField("XYZ: Coordinate W: Room Number");
        foreach (Vector4 coordinate in script.usedGridBlocks)
        {
            EditorGUILayout.Vector4Field("", coordinate);
        } 
       
        //foreach (Vector4 coordinate in script.usedGridBlock)
        base.OnInspectorGUI();
    }
}
#endif

