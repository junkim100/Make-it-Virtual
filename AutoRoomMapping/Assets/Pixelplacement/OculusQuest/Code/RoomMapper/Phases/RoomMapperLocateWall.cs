using UnityEngine;

namespace Pixelplacement.XRTools
{
    public class RoomMapperLocateWall : RoomMapperPhase
    {
        //Public Variables:
        public ChildActivator cursor;
        public LineRenderer connectionLine;

        public float groundCastDistance;
        public float groundHeight;
        public float groundDistance;
        
        //Private Variables:
        private float _lerpSpeed = 3.5f;
        private Vector3 _positionTarget;
        private float _maxCastDistance = 6f;
        private bool _onFloor;
        
        //Startup:
        protected override void Awake()
        {
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
            //parts:
            Plane floor = new Plane(Vector3.up, RoomAnchor.Instance.transform.position);
            Ray castRay = new Ray(ovrCameraRig.centerEyeAnchor.position, ovrCameraRig.centerEyeAnchor.forward);
            
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
            
            // height is camera's height
            // groundDistance is distance from camera to floor
            groundHeight = ovrCameraRig.centerEyeAnchor.position.y;
            groundDistance = Mathf.Sqrt(Mathf.Pow(groundCastDistance,2) - Mathf.Pow(groundHeight,2));
            
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

            Debug.Log("groundCastDistance: " + groundCastDistance);
            Debug.Log("groundHeight: " + groundHeight);
            Debug.Log("groundDistance: " + groundDistance);
            Debug.Log("transform.position: " + transform.position);
            Debug.Log("transform.rotation: " + transform.rotation);
            Debug.Log("Viewing Angle eulerAngles: " + ovrCameraRig.centerEyeAnchor.rotation.eulerAngles);
            Debug.Log("Viewing Angle: " + ovrCameraRig.centerEyeAnchor.rotation);
            
            //confirmation:
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onFloor)
                {
                    Next();
                }
            }
        }
    }
}