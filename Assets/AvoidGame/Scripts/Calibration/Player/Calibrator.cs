using AvoidGame.MediaPipe;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.Player
{
    /// <summary>
    /// Calculates the retargeting of the pose and holds the multiplier.
    /// </summary>
    [RequireComponent(typeof(PoseIK))]
    public class Calibrator : MonoBehaviour
    {
        [Inject] private PlayerInfo _playerInfo;

        [SerializeField] private PoseIK ik;

        private Vector3 _bodyMultiplier = Vector3.one;
        private float _floorY = 0f;

        private void Awake()
        {
            _bodyMultiplier = _playerInfo.BodyMultiplier;
            _floorY = _playerInfo.FloorHeight;
        }


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

            if (armLength < 0.001f || bodyHeight < 0.001f)
            {
                Debug.LogWarning("Pose estimation might be wrong. Using default values.");
                _floorY = 0f;
                _bodyMultiplier = Vector3.one;
            }
            else
            {
                _floorY = 1 - (leftHeel.Y + rightHeel.Y) * 0.5f;
                _bodyMultiplier.x = -Mathf.Abs(ik.leftWrist.position.x - ik.rightWrist.position.x) / armLength;
                _bodyMultiplier.y = Mathf.Abs(ik.leftElbow.position.y + ik.rightElbow.position.y -
                                        ik.rightFoot.position.y + ik.leftFoot.position.y) /
                                    bodyHeight;
                _bodyMultiplier.z = 0.5f;
            }

            _playerInfo.BodyMultiplier = _bodyMultiplier;
            _playerInfo.FloorHeight = _floorY;
        }


        private Vector3 ScaleBody(float x, float y, float z)
        {
            y = 1 - y;
            return Vector3.Scale(_bodyMultiplier, new Vector3(x - 0.5f, y - _floorY, z));
        }


        public void Retarget(Landmark[] landmarks)
        {
            if (landmarks.Length != 33) return;
            // get media pipe landmarks
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

            var hipY = (leftHip.Y + rightHip.Y) * 0.5f;

            // set ik positions
            ik.hip.localPosition = ScaleBody(
                (leftHip.X + rightHip.X) * 0.5f,
                (leftHip.Y + rightHip.Y) * 0.5f,
                (leftHip.Z + rightHip.Z) * 0.5f);
            ik.leftFoot.localPosition = ScaleBody(leftFoot.X, leftFoot.Y, leftFoot.Z);
            ik.rightFoot.localPosition = ScaleBody(rightFoot.X, rightFoot.Y, rightFoot.Z);
            ik.leftKnee.localPosition = ScaleBody(leftShin.X, leftShin.Y, leftShin.Z);
            ik.rightKnee.localPosition = ScaleBody(rightShin.X, rightShin.Y, rightShin.Z);

            ik.neck.localPosition = ScaleBody(
                (leftShoulder.X + rightShoulder.X) * 0.5f,
                (leftShoulder.Y + rightShoulder.Y) * 0.5f,
                (leftShoulder.Z + rightShoulder.Z) * 0.5f);
            ik.leftWrist.localPosition = ScaleBody(leftHand.X, leftHand.Y, leftHand.Z);
            ik.rightWrist.localPosition = ScaleBody(rightHand.X, rightHand.Y, rightHand.Z);
            ik.leftElbow.localPosition = ScaleBody(leftForearm.X, leftForearm.Y, leftForearm.Z);
            ik.rightElbow.localPosition = ScaleBody(rightForeArm.X, rightForeArm.Y, rightForeArm.Z);
        }
    }
}