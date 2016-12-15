using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneManager : MonoBehaviour
{

    public List<Transform> RuneTransform;
    public bool[] RuneUsed;

    // Use this for initialization
    void Start()
    {
        RuneUsed = new bool[RuneTransform.Count];
        for (int i = 0; i < RuneUsed.Length; i++)
        {
            RuneUsed[i] = false;
        }
    }

    //Return a random non-used rune
    public Transform GetRandomRune()
    {
        int randomIndex = Random.Range(0, RuneTransform.Count);
        if (RuneUsed[randomIndex])
        {
            return GetRandomRune();
        }
        else
        {
            RuneUsed[randomIndex] = true;
            return RuneTransform[randomIndex];
        }
    }

}

