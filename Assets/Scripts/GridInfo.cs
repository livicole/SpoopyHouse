﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class GridInfo : MonoBehaviour {

    [HideInInspector]
    public List<RoomInfo> usedGridBlocks;

    [SerializeField]
    public float gridSize;

    [SerializeField]
    public float blockLength;

    public float gridMax, gridMin = 0;

    public float moveTick;

	// Use this for initialization
	void Start () {
        
        Debug.Log("The list:" + usedGridBlocks);
        float length = (gridSize + 1) * blockLength;
	    transform.localScale = new Vector3 (length, 1, length);
        transform.position = new Vector3(gridSize * 10 / 2, 0, gridSize * 10 /2);
        gridMax = gridSize; gridMin = 0;

        
	}

    public void AddBlock(Vector3 coordinate, Transform transform)
    {
        RoomInfo newRoom = new RoomInfo();
        newRoom.Create(coordinate, transform);
        Debug.Log(newRoom.coordinate);
        Debug.Log(usedGridBlocks);
        usedGridBlocks.Add(newRoom);
    }

    public void RemoveBlock(Vector3 coordinate, Transform transform)
    {
        RoomInfo newRoom = new RoomInfo();
        newRoom.Create(coordinate, transform);

        usedGridBlocks.Remove(newRoom);
    }

    public void InitList()
    {
        usedGridBlocks = new List<RoomInfo>();
    }
}

public class RoomInfo{
    public Vector3 coordinate;
    public Transform room;

    public void Create(Vector3 coordinates, Transform roomTransform)
    {
        coordinate = coordinates;
        room = roomTransform;
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
        foreach (RoomInfo rooms in script.usedGridBlocks)
        {
            EditorGUILayout.Vector3Field("", rooms.coordinate);
            EditorGUILayout.ObjectField("Transform of above:", rooms.room, typeof(Object));
            EditorGUILayout.Space();
        } 
       
        //foreach (Vector4 coordinate in script.usedGridBlock)
        base.OnInspectorGUI();
    }
}
#endif

