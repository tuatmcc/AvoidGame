using AvoidGame.MediaPipe;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace AvoidGame.Calibration.Player
{
    /// <summary>
    /// Calculates the retargeting of the pose and holds the multiplier.
    /// </summary>
    [RequireComponent(typeof(IKVisualizer))]
    public class IKController : MonoBehaviour
    {
        [Inject] private PlayerInfo _playerInfo;

        [SerializeField] private IKVisualizer ikVisualizer;

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
                _bodyMultiplier.x = -Mathf.Abs(ikVisualizer.leftWrist.position.x - ikVisualizer.rightWrist.position.x) /
                                    armLength;
                _bodyMultiplier.y = Mathf.Abs(ikVisualizer.leftElbow.position.y + ikVisualizer.rightElbow.position.y -
                                        ikVisualizer.rightFoot.position.y + ikVisualizer.leftFoot.position.y) /
                                    bodyHeight;
                _bodyMultiplier.z = 0.5f;
            }

            _playerInfo.BodyMultiplier = _bodyMultiplier;
            _playerInfo.FloorHeight = _floorY;
        }


        private Vector3 ScaleBody(float x, float y, float z)
        {
            y = 1 - y;
            return Vector3.Scale(_bodyMultiplier, new Vector3(-x + 0.5f, y - _floorY, -z));
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

            var neckX = (leftShoulder.X + rightShoulder.X) * 0.5f;
            var neckY = (leftShoulder.Y + rightShoulder.Y) * 0.5f;
            var neckZ = (leftShoulder.Z + rightShoulder.Z) * 0.5f;
            var headY = neckY + (nose.Y - neckY) * 0.2f;

            var hipX = (leftHip.X + rightHip.X) * 0.5f;
            var hipY = (leftHip.Y + rightHip.Y) * 0.5f;
            var xBase = (leftHip.X + rightHip.X) * 0.5f - 0.5f;
            // var xBase = 0f;
            var zBase = (leftHip.Z + rightHip.Z) * 0.5f;

            var baseVec = new Vector3(hipX, 0, 0);

            // set ik positions
            ikVisualizer.hip.localPosition = baseVec + ScaleBody(
                (leftHip.X + rightHip.X) * 0.5f - xBase,
                (leftHip.Y + rightHip.Y) * 0.5f,
                (leftHip.Z + rightHip.Z) * 0.5f - zBase);
            transform.localPosition = baseVec +  ScaleBody((leftHip.X + rightHip.X) * 0.5f, 1, 0);
            ikVisualizer.leftFoot.localPosition = baseVec + ScaleBody(leftFoot.X - xBase, leftFoot.Y, leftFoot.Z - zBase);
            ikVisualizer.rightFoot.localPosition = baseVec + ScaleBody(rightFoot.X - xBase, rightFoot.Y, rightFoot.Z - zBase);
            ikVisualizer.leftKnee.localPosition = baseVec + ScaleBody(leftShin.X - xBase, leftShin.Y, leftShin.Z - zBase);
            ikVisualizer.rightKnee.localPosition = baseVec +  ScaleBody(rightShin.X - xBase, rightShin.Y, rightShin.Z - zBase);

            ikVisualizer.neck.localPosition =  baseVec + ScaleBody(neckX - xBase, neckY, neckZ - zBase);
            // ik.head.localPosition = ScaleBody(neckX - xBase, headY, neckZ - zBase);
            ikVisualizer.leftWrist.localPosition =  baseVec + ScaleBody(leftHand.X - xBase, leftHand.Y, leftHand.Z - zBase);
            ikVisualizer.rightWrist.localPosition = baseVec +  ScaleBody(rightHand.X - xBase, rightHand.Y, rightHand.Z - zBase);
            ikVisualizer.leftElbow.localPosition =
                baseVec +  ScaleBody(leftForearm.X - xBase, leftForearm.Y, leftForearm.Z - zBase);
            ikVisualizer.rightElbow.localPosition =
                baseVec + ScaleBody(rightForeArm.X - xBase, rightForeArm.Y, rightForeArm.Z - zBase);

            ikVisualizer.hip.localRotation = Quaternion.Euler(new Vector3(neckX, neckY, neckZ) - new Vector3(
                leftHip.X + rightHip.X,
                (leftHip.Y + rightHip.Y) / 2, (leftHip.Z + rightHip.Z) / 2));

            SetWristRotations(landmarks);
        }

        private void SetWristRotations(Landmark[] landmarks)
        {
            // Left wrist rotation
            var leftWrist = landmarks[(int)LandmarkIndex.LEFT_WRIST];
            var leftIndex = landmarks[(int)LandmarkIndex.LEFT_INDEX];
            var leftThumb = landmarks[(int)LandmarkIndex.LEFT_THUMB];

            var leftWristRotation = CalculateRotation(leftWrist, leftIndex, leftThumb);
            ikVisualizer.leftHand.localRotation = leftWristRotation;

            // Right wrist rotation
            var rightWrist = landmarks[(int)LandmarkIndex.RIGHT_WRIST];
            var rightIndex = landmarks[(int)LandmarkIndex.RIGHT_INDEX];
            var rightThumb = landmarks[(int)LandmarkIndex.RIGHT_THUMB];

            var rightWristRotation = CalculateRotation(rightWrist, rightIndex, rightThumb);
            ikVisualizer.rightHand.localRotation = rightWristRotation;
        }

        // Helper method to calculate quaternion rotation from three points
        private Quaternion CalculateRotation(Landmark wrist, Landmark index, Landmark thumb)
        {
            var wristPos = new Vector3(wrist.X, -wrist.Y, wrist.Z);
            var indexPos = new Vector3(index.X, -index.Y, index.Z);
            var thumbPos = new Vector3(thumb.X, -thumb.Y, thumb.Z);

            var forward = (indexPos - wristPos).normalized;
            var up = (thumbPos - wristPos).normalized;
            var rotation = Quaternion.LookRotation(forward, up);

            return rotation;
        }
    }
}