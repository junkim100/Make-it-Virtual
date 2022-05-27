using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOff : MonoBehaviour
{

    private Light[] lights;

    void Start()
    {
        lights = FindObjectsOfType(typeof(Light)) as Light[];
        foreach(Light light in lights) {
            if(light.tag == "Light") {
                light.enabled = false;
            }
        }
    }
}
