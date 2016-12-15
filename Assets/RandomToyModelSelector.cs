using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomToyModelSelector : MonoBehaviour {

    //List of all our toymodels
    public List<Transform> ToyModels;

    //Returns us a random model
    public Transform getRandomModel()
    {
        float i = Random.Range(0, ToyModels.Count - 0.1f);
        int randomIndex = (int)i;
        return ToyModels[randomIndex];
    }
}
