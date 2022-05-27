using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class lightToggle : MonoBehaviour
{
    // public OVRPassthroughLayer passthorugh;
    // public OVRInput.Button button;
    // public OVRInput.Controller controller;

    // public GameObject light;

    // void Update()
    // {
    //     if (OVRInput.GetDown(button, controller))
    //     {
    //         light.enabled = !light.enabled;
    //     }
    // }

    [SerializeField] private bool isLightOn;

    [SerializeField] private UnityEvent lightOnEvent;
    [SerializeField] private UnityEvent lightOffEvent;
    
    public void InteractSwitch()
    {
        if (!isLightOn)
        {
            isLightOn = true;
            lightOnEvent.Invoke();
        }
        else
        {
            isLightOn = false;
            lightOffEvent.Invoke();
        }
    }
}

