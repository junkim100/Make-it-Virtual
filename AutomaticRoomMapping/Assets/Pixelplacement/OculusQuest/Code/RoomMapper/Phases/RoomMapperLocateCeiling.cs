using System.Collections;
using UnityEngine;
using lemnel.AutoRoomMapper;

namespace Pixelplacement.XRTools
{
    public class RoomMapperLocateCeiling : RoomMapperPhase
    {
        //Public Variables:
        public RoomMapperLocateWall locateWall;
        public ChildActivator cursor;
        public LineRenderer connectionLine;

        public float ceilingDistance;
        public float ceilingHeight;
        public Vector3 firstCorner;

        public GameObject crosshair;
        public CrosshairTimer crosshairTimer;

        //Private Variables:
        private bool _aboveHead;
        private float _lerpSpeed = 3.5f;
        
        //Startup:
        protected override void Awake()
        {
            Debug.Log("Start Locate Ceiling");

            base.Awake();
            
            //refs:
            connectionLine = GetComponentInChildren<LineRenderer>(true);
        }

        private void OnEnable()
        {
            //sets:
            _aboveHead = false;
            transform.SetPositionAndRotation(locateWall.transform.position, locateWall.transform.rotation);
            cursor.Activate(0);
            crosshair.SetActive(true);
        }

        //Loops:
        private void Update()
        {
            Debug.Log("Inside Update");
            //parts:
            Plane edge = new Plane(-locateWall.transform.forward, locateWall.transform.position);
            Ray castRay = new Ray(ovrCameraRig.centerEyeAnchor.position, ovrCameraRig.centerEyeAnchor.forward);
            float castDistance;
            
            //cast:
            edge.Raycast(castRay, out castDistance);

            //current state:
            if (transform.position.y > ovrCameraRig.centerEyeAnchor.position.y)
            {
                crosshair.SetActive(true);
                if (!_aboveHead)
                {
                    _aboveHead = true;
                    cursor.Activate(1);
                }
            }
            else
            {
                crosshair.SetActive(false);
                if (_aboveHead)
                {
                    _aboveHead = false;
                    cursor.Activate(0);
                }
            }
            
            // position:
            Vector3 location = locateWall.transform.position;
            location.y = castRay.GetPoint(castDistance).y;
            transform.position = Vector3.Lerp(transform.position, location, Time.deltaTime * _lerpSpeed);

            //lines:
            if (_aboveHead)
            {
                Vector3 crosshairPosition = connectionLine.transform.InverseTransformPoint(crosshair.transform.position);
                connectionLine.SetPosition(0, crosshairPosition);
                connectionLine.SetPosition(3, crosshairPosition);
            }

            float groundHeight = ovrCameraRig.centerEyeAnchor.position.y;
            float viewAngle = 360 - ovrCameraRig.centerEyeAnchor.rotation.eulerAngles.x;
            float rad = viewAngle * Mathf.Deg2Rad;
            ceilingDistance = Mathf.Tan(rad) * locateWall.groundDistance;
            ceilingHeight = ceilingDistance + groundHeight;

            Debug.Log("Viewing Angle: " + viewAngle);
            Debug.Log("Ceiling Distance: " + ceilingDistance);
            Debug.Log("Ceiling Height: " + ceilingHeight);

            int timeleft = crosshairTimer.TimerStart();
            StartCoroutine( sleep(1.0f) );
            
            //confirmation:
            // if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            if (timeleft <= 0)
            {
                crosshairTimer.TimerReset();
                crosshair.SetActive(false);
                firstCorner = new Vector3(locateWall.transform.position.x, ceilingHeight, locateWall.transform.position.z);
                Next();
            }
        }

        private IEnumerator sleep(float seconds)
        {
            float prevAngle = ovrCameraRig.centerEyeAnchor.rotation.eulerAngles.x;
            yield return new WaitForSeconds(seconds);
            float currAngle = ovrCameraRig.centerEyeAnchor.rotation.eulerAngles.x;
            
            if (Mathf.Abs(prevAngle - currAngle) > 1.0f)
                crosshairTimer.TimerReset();
        }
    }
}