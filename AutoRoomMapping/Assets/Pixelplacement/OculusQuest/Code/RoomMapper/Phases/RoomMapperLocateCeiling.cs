using UnityEngine;

namespace Pixelplacement.XRTools
{
    public class RoomMapperLocateCeiling : RoomMapperPhase
    {
        //Public Variables:
        public RoomMapperLocateWall locateWall;
        public LineRenderer connectionLine;
        public ChildActivator cursor;

        public float ceilingDistance;
        public float ceilingHeight;
        public Vector3 firstCorner;

        //Private Variables:
        private bool _aboveHead;
        private float _lerpSpeed = 3.5f;
        
        //Startup:
        protected override void Awake()
        {
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
        }

        //Loops:
        private void Update()
        {
            //parts:
            Plane edge = new Plane(-locateWall.transform.forward, locateWall.transform.position);
            Ray castRay = new Ray(ovrCameraRig.centerEyeAnchor.position, ovrCameraRig.centerEyeAnchor.forward);
            float castDistance;
            
            //cast:
            edge.Raycast(castRay, out castDistance);

            //current state:
            if (transform.position.y > ovrCameraRig.centerEyeAnchor.position.y)
            {
                if (!_aboveHead)
                {
                    _aboveHead = true;
                    cursor.Activate(1);
                }
            }
            else
            {
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
                Vector3 headPosition = connectionLine.transform.InverseTransformPoint(ovrCameraRig.centerEyeAnchor.position);
                connectionLine.SetPosition(0, headPosition);
                connectionLine.SetPosition(3, headPosition);
            }

            float groundHeight = ovrCameraRig.centerEyeAnchor.position.y;
            float viewAngle = 360 - ovrCameraRig.centerEyeAnchor.rotation.eulerAngles.x;
            float rad = viewAngle * Mathf.Deg2Rad;
            ceilingDistance = Mathf.Tan(rad) * locateWall.groundDistance;
            ceilingHeight = ceilingDistance + groundHeight;

            Debug.Log("Viewing Angle: " + viewAngle);
            Debug.Log("Ceiling Distance: " + ceilingDistance);
            Debug.Log("Ceiling Height: " + ceilingHeight);
            
            //confirmation:
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_aboveHead)
                {
                    firstCorner = new Vector3(locateWall.transform.position.x, ceilingHeight, locateWall.transform.position.z);
                    Next();
                }
            }
        }
    }
}