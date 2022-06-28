using System.Collections;
using UnityEngine;
using lemnel.AutoRoomMapper;

namespace Pixelplacement.XRTools
{
    public class RoomMapperLocateWall : RoomMapperPhase
    {
        //Public Variables:
        public ChildActivator cursor;
        public LineRenderer connectionLine;
        public CrosshairTimer crosshairTimer;

        public float groundDistance;
        
        //Private Variables:
        private float _lerpSpeed = 3.5f;
        private Vector3 _positionTarget;
        private float _maxCastDistance = 6f;
        private bool _onFloor;

        void Start() {
            crosshairTimer.TimerSetup();
        }
        
        //Startup:
        protected override void Awake()
        {
            Debug.Log("Start Locate Wall");
            
            base.Awake();
            
            //sets:
            cursor.Activate(0);
        }

        private void OnEnable()
        {
            //sets:
            _onFloor = false;
            cursor.Activate(0);
        }

        //Loops:
        private void Update()
        {
            Debug.Log("Inside Update");
            //parts:
            Plane floor = new Plane(Vector3.up, RoomAnchor.Instance.transform.position);
            Ray castRay = new Ray(ovrCameraRig.centerEyeAnchor.position, ovrCameraRig.centerEyeAnchor.forward);
            float groundCastDistance;

            //cursor state:
            if (floor.Raycast(castRay, out groundCastDistance))
            {
                if (!_onFloor)
                {
                    _onFloor = true;
                    cursor.Activate(1);
                }
            }
            else
            {
                if (_onFloor)
                {
                    _onFloor = false;
                    cursor.Activate(0);
                }
            }
            
            //clamp:
            if (groundCastDistance <= 0 || groundCastDistance > _maxCastDistance)
            {
                groundCastDistance = _maxCastDistance;
            }
            
            //position (force to floor):
            Vector3 position = castRay.GetPoint(groundCastDistance);
            if (_onFloor)
            {
                position.y = RoomAnchor.Instance.transform.position.y;
                //snap to floor:
                cursor.transform.position = new Vector3(cursor.transform.position.x, RoomAnchor.Instance.transform.position.y, cursor.transform.position.z);
            }
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * _lerpSpeed);
            
            //rotation:
            if (_onFloor)
            {
                Vector3 flatForward = Vector3.ProjectOnPlane(ovrCameraRig.centerEyeAnchor.forward, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flatForward), Time.deltaTime * _lerpSpeed);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(ovrCameraRig.centerEyeAnchor.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _lerpSpeed);
            }
            
            //lines:
            if (_onFloor)
            {
                Vector3 headPosition = connectionLine.transform.InverseTransformPoint(ovrCameraRig.centerEyeAnchor.position);
                connectionLine.SetPosition(0, headPosition);
                connectionLine.SetPosition(3, headPosition);
            }
            
            // height is camera's height
            // groundDistance is distance from camera to floor
            float groundHeight = ovrCameraRig.centerEyeAnchor.position.y;
            groundDistance = Mathf.Sqrt(Mathf.Pow(groundCastDistance,2) - Mathf.Pow(groundHeight,2));

            Debug.Log("time: " + crosshairTimer.sec);
            Debug.Log("groundCastDistance: " + groundCastDistance);
            Debug.Log("groundHeight: " + groundHeight);
            Debug.Log("groundDistance: " + groundDistance);
            Debug.Log("Viewing Angle: " + ovrCameraRig.centerEyeAnchor.rotation.eulerAngles.x);

            StartCoroutine( sleep(1.0f) );
            
            //confirmation:
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onFloor)
                {
                    Next();
                }
            }
        }

        private IEnumerator sleep(float seconds)
        {
            float prevAngle = ovrCameraRig.centerEyeAnchor.rotation.eulerAngles.x;
            yield return new WaitForSeconds(seconds);
            float currAngle = ovrCameraRig.centerEyeAnchor.rotation.eulerAngles.x;
            if (Mathf.Abs(prevAngle - currAngle) < 1.0f)
                crosshairTimer.TimerStart();
            else
                crosshairTimer.TimerStop();
        }

    }
}