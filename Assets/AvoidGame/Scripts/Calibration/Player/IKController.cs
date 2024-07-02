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
        private float yBase = 0f;

        private void Awake()
        {
            _bodyMultiplier = _playerInfo.BodyMultiplier;
            yBase = _playerInfo.FloorHeight;
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
                yBase = 0f;
                _bodyMultiplier = Vector3.one;
            }
            else
            {
                yBase = (leftHeel.Y + rightHeel.Y) * 0.5f;
                _bodyMultiplier.x = -Mathf.Abs(ikVisualizer.leftWrist.position.x - ikVisualizer.rightWrist.position.x) /
                                    armLength;
                _bodyMultiplier.y = Mathf.Abs(ikVisualizer.leftElbow.position.y + ikVisualizer.rightElbow.position.y -
                                        ikVisualizer.rightFoot.position.y + ikVisualizer.leftFoot.position.y) /
                                    bodyHeight;
                _bodyMultiplier.z = 0.5f; // 適当
            }

            _playerInfo.BodyMultiplier = _bodyMultiplier;
            _playerInfo.FloorHeight = yBase;
        }


        /// <summary>
        /// x,y,z coords relative to hip
        /// </summary>
        /// <param name="relativeX"></param>
        /// <param name="relativeY"></param>
        /// <param name="relativeZ"></param>
        /// <returns></returns>
        private Vector3 ScaleBody(float relativeX, float relativeY, float relativeZ)
        {
            return Vector3.Scale(_bodyMultiplier, new Vector3(relativeX, relativeY - yBase, relativeZ));
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

            var hipY = (leftHip.Y + rightHip.Y) * 0.5f;
            var hipX = (leftHip.X + rightHip.X) * 0.5f;
            var hipZ = (leftHip.Z + rightHip.Z) * 0.5f;

            var baseVec = new Vector3(hipX, 0, 0);

            // set ik positions
            ikVisualizer.hip.localPosition = baseVec + ScaleBody(
                (leftHip.X + rightHip.X) * 0.5f - hipX,
                (leftHip.Y + rightHip.Y) * 0.5f,
                (leftHip.Z + rightHip.Z) * 0.5f - hipZ);
            ikVisualizer.leftFoot.localPosition = baseVec + ScaleBody(leftFoot.X - hipX, leftFoot.Y, leftFoot.Z - hipZ);
            ikVisualizer.rightFoot.localPosition = baseVec + ScaleBody(rightFoot.X - hipX, rightFoot.Y, rightFoot.Z - hipZ);
            ikVisualizer.leftKnee.localPosition = baseVec + ScaleBody(leftShin.X - hipX, leftShin.Y, leftShin.Z - hipZ);
            ikVisualizer.rightKnee.localPosition = baseVec +  ScaleBody(rightShin.X - hipX, rightShin.Y, rightShin.Z - hipZ);

            ikVisualizer.neck.localPosition =  baseVec + ScaleBody(neckX - hipX, neckY, neckZ - hipZ);
            // ik.head.localPosition = ScaleBody(neckX - xBase, headY, neckZ - zBase);
            ikVisualizer.leftWrist.localPosition =  baseVec + ScaleBody(leftHand.X - hipX, leftHand.Y, leftHand.Z - hipZ);
            ikVisualizer.rightWrist.localPosition = baseVec +  ScaleBody(rightHand.X - hipX, rightHand.Y, rightHand.Z - hipZ);
            ikVisualizer.leftElbow.localPosition =
                baseVec +  ScaleBody(leftForearm.X - hipX, leftForearm.Y, leftForearm.Z - hipZ);
            ikVisualizer.rightElbow.localPosition =
                baseVec + ScaleBody(rightForeArm.X - hipX, rightForeArm.Y, rightForeArm.Z - hipZ);

            ikVisualizer.hip.localRotation = Quaternion.Euler(new Vector3(neckX, neckY, neckZ) - new Vector3(
                leftHip.X + rightHip.X,
                (leftHip.Y + rightHip.Y) / 2, (leftHip.Z + rightHip.Z) / 2));

            SetWristRotations(landmarks);
        }

        private void SetWristRotations(Landmark[] landmarks)
        {
            // Left wrist rotation
            var leftWrist = landmarks[(int)LandmarkIndex.LEFT_WRIST];
            var leftPinky = landmarks[(int)LandmarkIndex.LEFT_PINKY];
            var leftIndex = landmarks[(int)LandmarkIndex.LEFT_INDEX];
            var leftThumb = landmarks[(int)LandmarkIndex.LEFT_THUMB];

            var leftWristRotation = CalculateRotation(leftWrist, leftPinky, leftIndex, leftThumb);
            ikVisualizer.leftWrist.localRotation = leftWristRotation;

            // Right wrist rotation
            var rightWrist = landmarks[(int)LandmarkIndex.RIGHT_WRIST];
            var rightPinky = landmarks[(int)LandmarkIndex.RIGHT_PINKY];
            var rightIndex = landmarks[(int)LandmarkIndex.RIGHT_INDEX];
            var rightThumb = landmarks[(int)LandmarkIndex.RIGHT_THUMB];

            var rightWristRotation = CalculateRotation(rightWrist, rightPinky, rightIndex, rightThumb);
            ikVisualizer.rightWrist.localRotation = rightWristRotation;
        }

        // Helper method to calculate quaternion rotation from three points
        private Quaternion CalculateRotation(Landmark wrist, Landmark pinky, Landmark index, Landmark thumb)
        {
            var wristPos = new Vector3(wrist.X, wrist.Y, wrist.Z);
            var pinkyPos = new Vector3(pinky.X, pinky.Y, pinky.Z);
            var indexPos = new Vector3(index.X, index.Y, index.Z);
            var thumbPos = new Vector3(thumb.X, thumb.Y, thumb.Z);

            var forward = (indexPos - wristPos).normalized;
            var up = (thumbPos - pinkyPos).normalized;
            var rotation = Quaternion.LookRotation(forward, up);

            return rotation;
        }
    }
}