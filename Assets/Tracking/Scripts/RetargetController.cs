using Tracking.MediaPipe;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tracking
{
    public class RetargetController : MonoBehaviour
    {
        public Vector3 neckMultiplier = Vector3.one;
        public Vector3 leftForearmMultiplier = Vector3.one;
        public Vector3 leftHandMultiplier = Vector3.one;
        public Vector3 rightForearmMultiplier = Vector3.one;
        public Vector3 rightHandMultiplier = Vector3.one;
        public Vector3 leftShinMultiplier = Vector3.one;
        public Vector3 leftFootMultiplier = Vector3.one;
        public Vector3 rightShinMultiplier = Vector3.one;
        public Vector3 rightFootMultiplier = Vector3.one;

        [SerializeField] private PoseIKHolder ik;

        public void CalcRetargetMultiplier(Landmark[] landmarks)
        {
            if (landmarks.Length != 33) return;
            var leftHand = landmarks[(int)LandmarkIndex.LEFT_WRIST];
            var rightHand = landmarks[(int)LandmarkIndex.RIGHT_WRIST];
            var leftFoot = landmarks[(int)LandmarkIndex.LEFT_HEEL];
            var rightFoot = landmarks[(int)LandmarkIndex.RIGHT_HEEL];
            var upperMultiplierX =
                Mathf.Abs((ik.leftHand.position.x - ik.rightFoot.position.x) / (leftHand.X - rightHand.X));
            var upperMultiplierY =
                Mathf.Abs(
                    (ik.leftHand.position.y - ik.leftFoot.position.y + ik.rightHand.position.y -
                     ik.rightFoot.position.y) / (leftHand.Y + rightHand.Y - leftFoot.Y - rightFoot.Y));

            neckMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);
            neckMultiplier = new Vector3(upperMultiplierX, 1.4f, 1);
            leftForearmMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);
            leftHandMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);
            rightForearmMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);
            rightHandMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);

            rightFootMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);
            rightShinMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);

            leftHandMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);
            leftShinMultiplier = new Vector3(upperMultiplierX, upperMultiplierY, upperMultiplierX);
        }

        public void Retarget(Landmark[] landmarks)
        {
            if (landmarks.Length != 33) return;

            var hight = 1.8f;
            var thickness = 0.1f;
            var leftShoulder = landmarks[(int)LandmarkIndex.LEFT_SHOULDER];
            var rightShoulder = landmarks[(int)LandmarkIndex.RIGHT_SHOULDER];
            var leftHand = landmarks[(int)LandmarkIndex.LEFT_WRIST];
            var rightHand = landmarks[(int)LandmarkIndex.RIGHT_WRIST];
            ik.neck.position = new Vector3(-neckMultiplier.x * ((leftShoulder.X + rightShoulder.X) / 2f - 0.5f),
                hight - neckMultiplier.y * (leftShoulder.Y + rightShoulder.Y) / 2f,
                neckMultiplier.z * (leftShoulder.Z + rightShoulder.Z) / 2f * thickness);
            ik.leftHand.position = new Vector3(-leftHandMultiplier.x * (leftHand.X - 0.5f),
                hight - leftHandMultiplier.y * leftHand.Y,
                leftHandMultiplier.z * leftHand.Z * thickness);
            ik.leftForearm.position = new Vector3(-leftForearmMultiplier.x * (leftHand.X - 0.5f),
                hight - leftForearmMultiplier.y * leftHand.Y,
                leftForearmMultiplier.z * leftHand.Z * thickness);
            ik.rightHand.position = new Vector3(-rightHandMultiplier.x * (rightHand.X - 0.5f),
                hight - rightHandMultiplier.y * rightHand.Y,
                rightHandMultiplier.z * rightHand.Z * thickness);
            ik.rightForearm.position = new Vector3(-rightForearmMultiplier.x * (rightHand.X - 0.5f),
                hight - rightForearmMultiplier.y * rightHand.Y,
                rightForearmMultiplier.z * rightHand.Z * thickness);

            ik.leftFoot.position = new Vector3(-leftFootMultiplier.x * (leftHand.X - 0.5f),
                hight - leftFootMultiplier.y * leftHand.Y,
                leftFootMultiplier.z * leftHand.Z * thickness);
            ik.leftShin.position = new Vector3(-leftShinMultiplier.x * (leftHand.X - 0.5f),
                hight - leftShinMultiplier.y * leftHand.Y,
                leftShinMultiplier.z * leftHand.Z * thickness);
        }
    }
}