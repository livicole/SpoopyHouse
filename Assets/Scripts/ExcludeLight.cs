using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExcludeLight : MonoBehaviour {

    public List<Light> Lights;
    public bool cullLights = true;

    void OnPreCull()
    {
        if(cullLights == true)
        {
            foreach (Light light in Lights)
            {
                light.enabled = false;
            }
        }
    }
	
	// Update is called once per frame
	void OnPostRender()
    { 
	    if(cullLights == true)
        {
            foreach (Light light in Lights)
            {
                light.enabled = true;
            }
        }
	}
}
