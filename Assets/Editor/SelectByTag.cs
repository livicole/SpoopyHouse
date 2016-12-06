﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class SelectByTag : MonoBehaviour {

	private static string SelectedTag = "Door";

	[MenuItem("Helpers/Select By Tag")]
	public static void SelectObjectsWithTag()
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag(SelectedTag);
		Selection.objects = objects;
	}
}
