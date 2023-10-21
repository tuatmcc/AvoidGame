using System.Collections.Generic;
using Tracking.MediaPipe;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tracking
{
    public class RetargetController : MonoBehaviour
    {
        public Vector3 bodyMultiplier = Vector3.one;
        public float floorY = 0f;

        [SerializeField] private PoseIKHolder ik;

        [SerializeField] private GameObject debugIKPrefab;

        public void CalcRetargetMultiplier(Landmark[] landmarks)
        {
            if (landmarks.Length != 33) return;

            var leftWrist = landmarks[(int)LandmarkIndex.LEFT_WRIST];
            var rightWrist = landmarks[(int)LandmarkIndex.RIGHT_WRIST];
            var leftHip = landmarks[(int)LandmarkIndex.LEFT_HIP];
            var rightHip = landmarks[(int)LandmarkIndex.RIGHT_HIP];
            var leftAnkle = landmarks[(int)LandmarkIndex.LEFT_ANKLE];
            var rightAnkle = landmarks[(int)LandmarkIndex.RIGHT_ANKLE];
            var leftHeel = landmarks[(int)LandmarkIndex.LEFT_HEEL];
            var rightHeel = landmarks[(int)LandmarkIndex.RIGHT_HEEL];
            var armLength = Mathf.Abs(leftWrist.X - rightWrist.X);
            var bodyHeight = Mathf.Abs(leftWrist.Y + rightWrist.Y - leftAnkle.Y - rightAnkle.Y);


            floorY = 1 - (leftHeel.Y + rightHeel.Y) / 2f;
            bodyMultiplier.x = -Mathf.Abs(ik.leftWrist.position.x - ik.rightWrist.position.x) / armLength;
            bodyMultiplier.y = Mathf.Abs(ik.leftElbow.position.y + ik.rightElbow.position.y -
                                   ik.rightFoot.position.y + ik.leftFoot.position.y) /
                               bodyHeight;
            bodyMultiplier.z = 0.5f;
        }

        private Vector3 ScaleLowerBody(float x, float y, float z)
        {
            y = 1 - y;
            return Vector3.Scale(bodyMultiplier, new Vector3(x - 0.5f, y - floorY, z));
        }

        private Vector3 ScaleUpperBody(float x, float y, float z, float hipY)
        {
            y = 1 - y;
            return Vector3.Scale(bodyMultiplier, new Vector3(x - 0.5f, y - floorY, z));
        }

        public void Retarget(Landmark[] landmarks)
        {
            if (landmarks.Length != 33) return;

            var nose = landmarks[(int)LandmarkIndex.NOSE];
            var leftHip = landmarks[(int)LandmarkIndex.LEFT_HIP];
            var rightHip = landmarks[(int)LandmarkIndex.RIGHT_HIP];
            var leftFoot = landmarks[(int)LandmarkIndex.LEFT_HEEL];
            var leftShin = landmarks[(int)LandmarkIndex.LEFT_KNEE];
            var rightFoot = landmarks[(int)LandmarkIndex.RIGHT_HEEL];
            var rightShin = landmarks[(int)LandmarkIndex.RIGHT_KNEE];
            var leftShoulder = landmarks[(int)LandmarkIndex.LEFT_SHOULDER];
            var rightShoulder = landmarks[(int)LandmarkIndex.RIGHT_SHOULDER];
            var leftHand = landmarks[(int)LandmarkIndex.LEFT_WRIST];
            var rightHand = landmarks[(int)LandmarkIndex.RIGHT_WRIST];
            var leftForearm = landmarks[(int)LandmarkIndex.LEFT_ELBOW];
            var rightForeArm = landmarks[(int)LandmarkIndex.RIGHT_ELBOW];

            var hipY = (leftHip.Y + rightHip.Y) / 2f;

            // lower body
            ik.hip.position = ScaleLowerBody(
                (leftHip.X + rightHip.X) / 2f,
                (leftHip.Y + rightHip.Y) / 2f,
                (leftHip.Z + rightHip.Z) / 2f);
            ik.leftFoot.position = ScaleLowerBody(leftFoot.X, leftFoot.Y, leftFoot.Z);
            ik.rightFoot.position = ScaleLowerBody(rightFoot.X, rightFoot.Y, rightFoot.Z);
            ik.leftKnee.position = ScaleLowerBody(leftShin.X, leftShin.Y, leftShin.Z);
            ik.rightKnee.position = ScaleLowerBody(rightShin.X, rightShin.Y, rightShin.Z);

            // upper body
            ik.neck.position = ScaleUpperBody(
                (leftShoulder.X + rightShoulder.X) / 2f,
                (leftShoulder.Y + rightShoulder.Y) / 2f,
                (leftShoulder.Z + rightShoulder.Z) / 2f, hipY);
            ik.leftWrist.position = ScaleUpperBody(leftHand.X, leftHand.Y, leftHand.Z, hipY);
            ik.rightWrist.position = ScaleUpperBody(rightHand.X, rightHand.Y, rightHand.Z, hipY);
            ik.leftElbow.position = ScaleUpperBody(leftForearm.X, leftForearm.Y, leftForearm.Z, hipY);
            ik.rightElbow.position = ScaleUpperBody(rightForeArm.X, rightForeArm.Y, rightForeArm.Z, hipY);
        }
    }
}