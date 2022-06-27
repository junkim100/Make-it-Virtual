using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Camera CameraFacing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = CameraFacing.transform.position + CameraFacing.transform.forward * 0.5f;
        transform.LookAt(CameraFacing.transform.position);
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
