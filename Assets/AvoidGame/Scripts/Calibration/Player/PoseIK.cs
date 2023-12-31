using System.Collections.Generic;
using UnityEngine;

namespace AvoidGame.Calibration.Player
{
    /// <summary>
    /// Holds the IK points (and visualizes them)
    /// </summary>
    public class PoseIK : MonoBehaviour
    {
        [SerializeField] private bool debugIK = true;
        [SerializeField] private GameObject ikDebugPointPrefab;

        public Transform neck;

        // public Transform head;
        public Transform hip;
        public Transform leftShoulder;
        public Transform rightShoulder;
        public Transform leftElbow;
        public Transform rightElbow;
        public Transform leftWrist;
        public Transform rightWrist;
        public Transform leftKnee;
        public Transform rightKnee;
        public Transform leftFoot;
        public Transform rightFoot;

        private readonly List<Transform> _bones = new List<Transform>();
        private readonly List<Transform> _debugPoints = new List<Transform>();

        private void Start()
        {
            if (debugIK)
            {
                InstantiateIKDebugPoints();
            }
        }

        private void Update()
        {
            if (debugIK)
            {
                UpdateIKDebugPoints();
            }
        }

        private void InstantiateIKDebugPoints()
        {
            _bones.Add(neck);
            // _bones.Add(head);
            _bones.Add(hip);
            _bones.Add(leftElbow);
            _bones.Add(leftWrist);
            _bones.Add(rightElbow);
            _bones.Add(rightWrist);
            _bones.Add(leftKnee);
            _bones.Add(leftFoot);
            _bones.Add(rightKnee);
            _bones.Add(rightFoot);

            foreach (var _ in _bones)
            {
                _debugPoints.Add(Instantiate(ikDebugPointPrefab).transform);
            }
        }

        private void UpdateIKDebugPoints()
        {
            for (var i = 0; i < _debugPoints.Count; i++)
            {
                _debugPoints[i].position = _bones[i].position;
            }
        }
    }
}