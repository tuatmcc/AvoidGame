using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tracking
{
    public class Avatar : MonoBehaviour
    {
        public Camera cam;
        public Server server;
        public Animator animator;
        public LayerMask ground;
        public bool footTracking = true;
        public float footGroundOffset = .1f;

        private Dictionary<HumanBodyBones, CalibrationData> parentCalibrationData =
            new Dictionary<HumanBodyBones, CalibrationData>();

        private Quaternion initialRotation;
        private Vector3 initialPosition;
        private Quaternion targetRot;
        private CalibrationData spineUpDown, hipsTwist, chest, head;

        private void Start()
        {
            initialRotation = transform.rotation;
            initialPosition = transform.position;
        }

        public void Calibrate()
        {
            // Here we store the values of variables required to do the correct rotations at runtime.

            parentCalibrationData.Clear();

            // Manually setting calibration data for the spine chain as we want really specific control over that.
            spineUpDown = new CalibrationData(animator.GetBoneTransform(HumanBodyBones.Spine),
                animator.GetBoneTransform(HumanBodyBones.Neck),
                server.GetVirtualHip(), server.GetVirtualNeck());
            hipsTwist = new CalibrationData(animator.GetBoneTransform(HumanBodyBones.Hips),
                animator.GetBoneTransform(HumanBodyBones.Hips),
                server.GetLandmark(LandmarkIndex.RIGHT_HIP), server.GetLandmark(LandmarkIndex.LEFT_HIP));
            chest = new CalibrationData(animator.GetBoneTransform(HumanBodyBones.Chest),
                animator.GetBoneTransform(HumanBodyBones.Chest),
                server.GetLandmark(LandmarkIndex.RIGHT_HIP), server.GetLandmark(LandmarkIndex.LEFT_HIP));
            head = new CalibrationData(animator.GetBoneTransform(HumanBodyBones.Neck),
                animator.GetBoneTransform(HumanBodyBones.Head),
                server.GetVirtualNeck(), server.GetLandmark(LandmarkIndex.NOSE));

            // Adding calibration data automatically for the rest of the bones.
            AddCalibration(HumanBodyBones.RightUpperArm, HumanBodyBones.RightLowerArm,
                server.GetLandmark(LandmarkIndex.RIGHT_SHOULDER), server.GetLandmark(LandmarkIndex.RIGHT_ELBOW));
            AddCalibration(HumanBodyBones.RightLowerArm, HumanBodyBones.RightHand,
                server.GetLandmark(LandmarkIndex.RIGHT_ELBOW), server.GetLandmark(LandmarkIndex.RIGHT_WRIST));

            AddCalibration(HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg,
                server.GetLandmark(LandmarkIndex.RIGHT_HIP), server.GetLandmark(LandmarkIndex.RIGHT_KNEE));
            AddCalibration(HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot,
                server.GetLandmark(LandmarkIndex.RIGHT_KNEE), server.GetLandmark(LandmarkIndex.RIGHT_ANKLE));

            AddCalibration(HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftLowerArm,
                server.GetLandmark(LandmarkIndex.LEFT_SHOULDER), server.GetLandmark(LandmarkIndex.LEFT_ELBOW));
            AddCalibration(HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftHand,
                server.GetLandmark(LandmarkIndex.LEFT_ELBOW), server.GetLandmark(LandmarkIndex.LEFT_WRIST));

            AddCalibration(HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg,
                server.GetLandmark(LandmarkIndex.LEFT_HIP), server.GetLandmark(LandmarkIndex.LEFT_KNEE));
            AddCalibration(HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot,
                server.GetLandmark(LandmarkIndex.LEFT_KNEE), server.GetLandmark(LandmarkIndex.LEFT_ANKLE));

            if (footTracking)
            {
                AddCalibration(HumanBodyBones.LeftFoot, HumanBodyBones.LeftToes,
                    server.GetLandmark(LandmarkIndex.LEFT_ANKLE), server.GetLandmark(LandmarkIndex.LEFT_FOOT_INDEX));
                AddCalibration(HumanBodyBones.RightFoot, HumanBodyBones.RightToes,
                    server.GetLandmark(LandmarkIndex.RIGHT_ANKLE), server.GetLandmark(LandmarkIndex.RIGHT_FOOT_INDEX));
            }

            animator.enabled = false; // disable animator to stop interference.
        }

        private void AddCalibration(HumanBodyBones parent, HumanBodyBones child, Transform trackParent,
            Transform trackChild)
        {
            parentCalibrationData.Add(parent,
                new CalibrationData(animator.GetBoneTransform(parent), animator.GetBoneTransform(child),
                    trackParent, trackChild));
        }

        private void Update()
        {
            // Adjust the vertical position of the avatar to keep it approximately grounded.
            if (parentCalibrationData.Count > 0)
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
                    initialPosition + Vector3.up * displacement + Vector3.up * footGroundOffset,
                    Time.deltaTime * 5f);
            }

            // Compute the new rotations for each limbs of the avatar using the calibration datas we created before.
            foreach (var i in parentCalibrationData)
            {
                Quaternion deltaRotTracked = Quaternion.FromToRotation(i.Value.initialDir, i.Value.CurrentDirection);
                i.Value.parent.rotation = deltaRotTracked * i.Value.initialRotation;
            }

            // Deal with spine chain as a special case.
            if (parentCalibrationData.Count > 0)
            {
                Vector3 hd = head.CurrentDirection;
                // Some are partial rotations which we can stack together to specify how much we should rotate.
                Quaternion headr = Quaternion.FromToRotation(head.initialDir, hd);
                Quaternion twist = Quaternion.FromToRotation(hipsTwist.initialDir,
                    Vector3.Slerp(hipsTwist.initialDir, hipsTwist.CurrentDirection, .25f));
                Quaternion updown = Quaternion.FromToRotation(spineUpDown.initialDir,
                    Vector3.Slerp(spineUpDown.initialDir, spineUpDown.CurrentDirection, .25f));

                // Compute the final rotations.
                Quaternion h = updown * updown * updown * twist * twist;
                Quaternion s = h * twist * updown;
                Quaternion c = s * twist * twist;
                float speed = 10f;
                hipsTwist.Tick(h * hipsTwist.initialRotation, speed);
                spineUpDown.Tick(s * spineUpDown.initialRotation, speed);
                chest.Tick(c * chest.initialRotation, speed);
                head.Tick(updown * twist * headr * head.initialRotation, speed);

                // For additional responsiveness, we rotate the entire transform slightly based on the hips.
                Vector3 d = Vector3.Slerp(hipsTwist.initialDir, hipsTwist.CurrentDirection, .25f);
                d.y *= 0.5f;
                Quaternion deltaRotTracked = Quaternion.FromToRotation(hipsTwist.initialDir, d);
                targetRot = deltaRotTracked * initialRotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * speed);

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
    }
}