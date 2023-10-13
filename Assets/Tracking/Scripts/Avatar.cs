using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tracking
{
    public class Avatar : MonoBehaviour
    {
        [SerializeField] private Server server;
        public Transform bodyParent;
        public GameObject landmarkPrefab;
        public GameObject headPrefab;
        public Camera cam;
        public float maxSpeed = 50f;
        public float landmarkScale = 1f;
        public bool enableHead = false;
        public Animator animator;
        public LayerMask ground;
        public bool footTracking = true;
        public float multiplier = 10f;
        public float footGroundOffset = .1f;
        private Body _body;
        private Transform _virtualNeck;
        private Transform _virtualHip;
        public int samplesForPose = 1;

        private readonly Dictionary<HumanBodyBones, CalibrationData> _parentCalibrationData =
            new Dictionary<HumanBodyBones, CalibrationData>();

        private Quaternion _initialRotation;
        private Vector3 _initialPosition;
        private Quaternion _targetRot;
        private CalibrationData _spineUpDown, _hipsTwist, _chest, _head;

        private void Start()
        {
            _initialRotation = transform.rotation;
            _initialPosition = transform.position;

            _body = new Body(bodyParent, landmarkPrefab, landmarkScale, headPrefab);
            _virtualNeck = new GameObject("VirtualNeck").transform;
            _virtualHip = new GameObject("VirtualHip").transform;
        }

        public void Calibrate()
        {
            // Here we store the values of variables required to do the correct rotations at runtime.

            _parentCalibrationData.Clear();

            // Manually setting calibration data for the spine chain as we want really specific control over that.
            _spineUpDown = new CalibrationData(animator.GetBoneTransform(HumanBodyBones.Spine),
                animator.GetBoneTransform(HumanBodyBones.Neck), _virtualHip, _virtualNeck);
            _hipsTwist = new CalibrationData(animator.GetBoneTransform(HumanBodyBones.Hips),
                animator.GetBoneTransform(HumanBodyBones.Hips),
                GetLandmark(LandmarkIndex.RIGHT_HIP), GetLandmark(LandmarkIndex.LEFT_HIP));
            _chest = new CalibrationData(animator.GetBoneTransform(HumanBodyBones.Chest),
                animator.GetBoneTransform(HumanBodyBones.Chest),
                GetLandmark(LandmarkIndex.RIGHT_HIP), GetLandmark(LandmarkIndex.LEFT_HIP));
            _head = new CalibrationData(animator.GetBoneTransform(HumanBodyBones.Neck),
                animator.GetBoneTransform(HumanBodyBones.Head),
                _virtualNeck, GetLandmark(LandmarkIndex.NOSE));

            // Adding calibration data automatically for the rest of the bones.
            AddCalibration(HumanBodyBones.RightUpperArm, HumanBodyBones.RightLowerArm,
                GetLandmark(LandmarkIndex.RIGHT_SHOULDER), GetLandmark(LandmarkIndex.RIGHT_ELBOW));
            AddCalibration(HumanBodyBones.RightLowerArm, HumanBodyBones.RightHand,
                GetLandmark(LandmarkIndex.RIGHT_ELBOW), GetLandmark(LandmarkIndex.RIGHT_WRIST));

            AddCalibration(HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg,
                GetLandmark(LandmarkIndex.RIGHT_HIP), GetLandmark(LandmarkIndex.RIGHT_KNEE));
            AddCalibration(HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot,
                GetLandmark(LandmarkIndex.RIGHT_KNEE), GetLandmark(LandmarkIndex.RIGHT_ANKLE));

            AddCalibration(HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftLowerArm,
                GetLandmark(LandmarkIndex.LEFT_SHOULDER), GetLandmark(LandmarkIndex.LEFT_ELBOW));
            AddCalibration(HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftHand,
                GetLandmark(LandmarkIndex.LEFT_ELBOW), GetLandmark(LandmarkIndex.LEFT_WRIST));

            AddCalibration(HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg,
                GetLandmark(LandmarkIndex.LEFT_HIP), GetLandmark(LandmarkIndex.LEFT_KNEE));
            AddCalibration(HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot,
                GetLandmark(LandmarkIndex.LEFT_KNEE), GetLandmark(LandmarkIndex.LEFT_ANKLE));

            if (footTracking)
            {
                AddCalibration(HumanBodyBones.LeftFoot, HumanBodyBones.LeftToes,
                    GetLandmark(LandmarkIndex.LEFT_ANKLE), GetLandmark(LandmarkIndex.LEFT_FOOT_INDEX));
                AddCalibration(HumanBodyBones.RightFoot, HumanBodyBones.RightToes,
                    GetLandmark(LandmarkIndex.RIGHT_ANKLE), GetLandmark(LandmarkIndex.RIGHT_FOOT_INDEX));
            }

            animator.enabled = false; // disable animator to stop interference.
        }

        private void AddCalibration(HumanBodyBones parent, HumanBodyBones child, Transform trackParent,
            Transform trackChild)
        {
            _parentCalibrationData.Add(parent,
                new CalibrationData(animator.GetBoneTransform(parent), animator.GetBoneTransform(child),
                    trackParent, trackChild));
        }

        private Transform GetVirtualHip()
        {
            return _virtualHip;
        }

        private Transform GetVirtualNeck()
        {
            return _virtualNeck;
        }

        private Transform GetLandmark(LandmarkIndex mark)
        {
            return _body.instances[(int)mark].transform;
        }

        private void UpdateBody()
        {
            for (int i = 0; i < LANDMARK_COUNT; ++i)
            {
                if (_body.positionsBuffer[i].accumulatedValuesCount < samplesForPose)
                    continue;

                _body.localPositionTargets[i] = _body.positionsBuffer[i].value /
                    (float)_body.positionsBuffer[i].accumulatedValuesCount * multiplier;
                _body.positionsBuffer[i] = new AccumulatedBuffer(Vector3.zero, 0);
            }

            Vector3 offset = Vector3.zero;
            for (int i = 0; i < LANDMARK_COUNT; ++i)
            {
                Vector3 p = _body.localPositionTargets[i] - offset;
                _body.instances[i].transform.localPosition =
                    Vector3.MoveTowards(_body.instances[i].transform.localPosition, p, Time.deltaTime * maxSpeed);
            }

            _virtualNeck.transform.position = (_body.instances[(int)LandmarkIndex.RIGHT_SHOULDER].transform.position +
                                               _body.instances[(int)LandmarkIndex.LEFT_SHOULDER].transform.position) /
                                              2f;
            _virtualHip.transform.position = (_body.instances[(int)LandmarkIndex.RIGHT_HIP].transform.position +
                                              _body.instances[(int)LandmarkIndex.LEFT_HIP].transform.position) / 2f;
        }

        private void Update()
        {
            // Adjust the vertical position of the avatar to keep it approximately grounded.
            if (_parentCalibrationData.Count > 0)
            {
                float displacement = 0;
                RaycastHit h1;
                if (Physics.Raycast(animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, Vector3.down, out h1,
                        100f, ground, QueryTriggerInteraction.Ignore))
                {
                    displacement = (h1.point - animator.GetBoneTransform(HumanBodyBones.LeftFoot).position).y;
                }

                if (Physics.Raycast(animator.GetBoneTransform(HumanBodyBones.RightFoot).position, Vector3.down, out h1,
                        100f, ground, QueryTriggerInteraction.Ignore))
                {
                    float displacement2 = (h1.point - animator.GetBoneTransform(HumanBodyBones.RightFoot).position).y;
                    if (Mathf.Abs(displacement2) < Mathf.Abs(displacement))
                    {
                        displacement = displacement2;
                    }
                }

                transform.position = Vector3.Lerp(transform.position,
                    _initialPosition + Vector3.up * displacement + Vector3.up * footGroundOffset,
                    Time.deltaTime * 5f);

                UpdateBody();
            }


            // Compute the new rotations for each limbs of the avatar using the calibration datas we created before.
            foreach (var i in _parentCalibrationData)
            {
                Quaternion deltaRotTracked = Quaternion.FromToRotation(i.Value.initialDir, i.Value.CurrentDirection);
                i.Value.parent.rotation = deltaRotTracked * i.Value.initialRotation;
            }

            // Deal with spine chain as a special case.
            if (_parentCalibrationData.Count > 0)
            {
                Vector3 hd = _head.CurrentDirection;
                // Some are partial rotations which we can stack together to specify how much we should rotate.
                Quaternion headr = Quaternion.FromToRotation(_head.initialDir, hd);
                Quaternion twist = Quaternion.FromToRotation(_hipsTwist.initialDir,
                    Vector3.Slerp(_hipsTwist.initialDir, _hipsTwist.CurrentDirection, .25f));
                Quaternion updown = Quaternion.FromToRotation(_spineUpDown.initialDir,
                    Vector3.Slerp(_spineUpDown.initialDir, _spineUpDown.CurrentDirection, .25f));

                // Compute the final rotations.
                Quaternion h = updown * updown * updown * twist * twist;
                Quaternion s = h * twist * updown;
                Quaternion c = s * twist * twist;
                float speed = 10f;
                _hipsTwist.Tick(h * _hipsTwist.initialRotation, speed);
                _spineUpDown.Tick(s * _spineUpDown.initialRotation, speed);
                _chest.Tick(c * _chest.initialRotation, speed);
                _head.Tick(updown * twist * headr * _head.initialRotation, speed);

                // For additional responsiveness, we rotate the entire transform slightly based on the hips.
                Vector3 d = Vector3.Slerp(_hipsTwist.initialDir, _hipsTwist.CurrentDirection, .25f);
                d.y *= 0.5f;
                Quaternion deltaRotTracked = Quaternion.FromToRotation(_hipsTwist.initialDir, d);
                _targetRot = deltaRotTracked * _initialRotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, Time.deltaTime * speed);

                // The tracking of the camera.
                if (cam)
                {
                    Quaternion q = Quaternion.LookRotation(
                        (animator.GetBoneTransform(HumanBodyBones.Chest).transform.position - cam.transform.position)
                        .normalized, Vector3.up);
                    cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, q, Time.deltaTime * 3f);
                }
            }
        }

        const int LANDMARK_COUNT = 33;
        const int LINES_COUNT = 11;
    }
}