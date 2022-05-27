using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightonoff : MonoBehaviour
{

    public GameObject txtToDisplay;             //display the UI text
    
    private bool PlayerInZone;                  //check if the player is in trigger

    public OVRPassthroughLayer passthorugh;
    public OVRInput.Button button;
    public OVRInput.Controller controller;

    public GameObject light1;
    public GameObject light2;
    public GameObject light3;
    public GameObject light4;

    private void Start()
    {
        light1.SetActive(false);
        light2.SetActive(false);
        light3.SetActive(false);
        light4.SetActive(false);

        PlayerInZone = false;                   //player not in zone       
        txtToDisplay.SetActive(false);
    }

    private void Update()
    {
        if (OVRInput.GetDown(button, controller))           //if in zone and press F key
        {
            light1.SetActive(!light1.activeSelf);
            light2.SetActive(!light2.activeSelf);
            light3.SetActive(!light3.activeSelf);
            light4.SetActive(!light4.activeSelf);

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


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class lightonoff : MonoBehaviour
// {
//     public OVRPassthroughLayer passthorugh;
//     public OVRInput.Button button;
//     public OVRInput.Controller controller;

//     private bool PlayerInZone;                  //check if the player is in trigger

//     public GameObject light1;
//     public GameObject light2;
//     public GameObject light3;
//     public GameObject light4;

//     private void Start()
//     {

//         PlayerInZone = false;
//     }

//     private void Update()
//     {
//         if (PlayerInZone && OVRInput.GetDown(button, controller))           //if in zone and press F key
//         {
//             light1.SetActive(!light1.activeSelf);
//             light2.SetActive(!light2.activeSelf);
//             light3.SetActive(!light3.activeSelf);
//             light4.SetActive(!light4.activeSelf);

//             gameObject.GetComponent<Animator>().Play("switch");
//         }
//     }
// }