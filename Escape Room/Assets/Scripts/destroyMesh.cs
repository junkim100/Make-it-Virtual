using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyMesh : MonoBehaviour
{
    public OVRPassthroughLayer passthorugh;
    public OVRInput.Button button;
    public OVRInput.Controller controller;
    void Update()
    {
        if (OVRInput.GetDown(button, controller))
        {
            Destroy(gameObject);
        }
    }
}
