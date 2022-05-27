using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLightToggle : MonoBehaviour
{

    // function EnableLights (enable : boolean) {
    // for (light in FindObjectsOfType(Light)) light.enabled = enable;
    // }

    public GameObject txtToDisplay;             //display the UI text
    
    private bool PlayerInZone;                  //check if the player is in trigger

    public OVRPassthroughLayer passthorugh;
    public OVRInput.Button button;
    public OVRInput.Controller controller;

    private Light[] lights;

    private void Start()
    {
        lights = FindObjectsOfType(typeof(Light)) as Light[];
        foreach(Light light in lights) {
            if(light.tag == "Light") {
                light.enabled = false;
            }
        }

        PlayerInZone = false;                   //player not in zone       
        txtToDisplay.SetActive(false);
    }

    private void Update()
    {
        if (OVRInput.GetDown(button, controller))
        {
            foreach(Light light in lights) {
                if(light.tag == "Light") {
                    light.enabled = !light.enabled;
                }
            }
            gameObject.GetComponent<Animator>().Play("switch");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")     //if player in zone
        {
            txtToDisplay.SetActive(true);
            PlayerInZone = true;
        }
     }
    

    private void OnTriggerExit(Collider other)     //if player exit zone
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInZone = false;
            txtToDisplay.SetActive(false);
        }
    }
}
