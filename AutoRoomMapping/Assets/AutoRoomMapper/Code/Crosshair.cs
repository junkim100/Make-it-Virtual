using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lemnel.AutoRoomMapper
{
    public class Crosshair : MonoBehaviour
    {
        public Camera CameraFacing;

        void Update()
        {
            transform.position = CameraFacing.transform.position + CameraFacing.transform.forward * 0.5f;
            transform.LookAt(CameraFacing.transform.position);
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
}