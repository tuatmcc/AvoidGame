using System.Collections.Generic;
using AvoidGame.Calibration.MediaPipe;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration
{
    /// <summary>
    /// Debug class for controlling the whole process
    /// </summary>
    [RequireComponent(typeof(PoseIKHolder))]
    public class Calibrator : MonoBehaviour
    {
        [SerializeField] private bool debugLandmark = true;
        [SerializeField] private float retargetingTime = 5f;

        [SerializeField] private GameObject landmarkPrefab;

        [SerializeField] private PoseIKHolder poseIKHolder;

        private Receiver _receiver;

        [Inject] private GameStateManager _gameStateManager;

        private RetargetController _retargetController;

        private readonly PoseAccumulator _poseAccumulator = new PoseAccumulator();
        private bool _retargetStarted = false;
        private bool _retargetFinished = false;
        private float _timeElapsed = 0f;
        private readonly List<GameObject> _debugLandmark = new List<GameObject>();

        private void Awake()
        {
            _receiver = _gameStateManager.Receiver;
            _retargetController = _gameStateManager.RetargetController;
            _retargetController.IK = poseIKHolder;
        }

        private void Start()
        {
            if (landmarkPrefab && debugLandmark)
            {
                for (int i = 0; i < 33; i++)
                    _debugLandmark.Add(Instantiate(landmarkPrefab));
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_retargetStarted)
            {
                _retargetStarted = true;
            }

            if (!_retargetStarted) return;

            _timeElapsed += Time.deltaTime;

            if (_timeElapsed <= retargetingTime)
            {
                if (_receiver.ReceivedMessage != null)
                    _poseAccumulator.AccumulateLandmarks(
                        JsonConvert.DeserializeObject<Landmark[]>(_receiver.ReceivedMessage));
                return;
            }

            if (_timeElapsed > retargetingTime && !_retargetFinished)
            {
                _retargetFinished = true;
                _retargetController.CalcRetargetMultiplier(_poseAccumulator.GetAverageLandmarks());
                return;
            }

            var landmarks = JsonConvert.DeserializeObject<Landmark[]>(_receiver.ReceivedMessage);
            _retargetController.Retarget(landmarks);

            if (landmarkPrefab)
                DrawDebugLandmark(landmarks);
        }

        private void DrawDebugLandmark(IReadOnlyList<Landmark> landmarks)
        {
            if (!landmarkPrefab)
            {
                Debug.Log("landmark prefab is not set");
                return;
            }

            for (var i = 0; i < 33; i++)
            {
                _debugLandmark[i].transform.position = new Vector3(landmarks[i].X, landmarks[i].Y, landmarks[i].Z);
            }
        }
    }
}