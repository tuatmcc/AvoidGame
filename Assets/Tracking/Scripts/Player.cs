using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tracking.MediaPipe;
using UnityEngine;
using Newtonsoft.Json;

namespace Tracking
{
    public class Player : MonoBehaviour
    {
        private Receiver _receiver;

        [SerializeField] private PoseIKHolder poseIKHolder;

        [SerializeField] private RetargetController retargetController;

        [SerializeField] private GameObject landmark;

        private readonly BasicPoseAccumulator _basicPoseAccumulator = new BasicPoseAccumulator();
        private bool _retargetStarted = false;
        private bool _retargetFinished = false;
        private float _timeElapsed = 0f;
        private readonly List<GameObject> _debugLandmark = new List<GameObject>();


        private void Start()
        {
            _receiver = new Receiver();
            var token = this.GetCancellationTokenOnDestroy();
            _receiver.StartReceiver(token).Forget();

            if (landmark)
            {
                for (int i = 0; i < 33; i++)
                    _debugLandmark.Add(Instantiate(landmark));
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

            if (_timeElapsed <= 5f)
            {
                if (_receiver.ReceivedMessage != null)
                    _basicPoseAccumulator.AccumulateLandmarks(
                        JsonConvert.DeserializeObject<Landmark[]>(_receiver.ReceivedMessage));
                return;
            }

            if (_timeElapsed > 5f && !_retargetFinished)
            {
                _retargetFinished = true;
                retargetController.CalcRetargetMultiplier(_basicPoseAccumulator.GetAverageLandmarks());
                return;
            }

            var landmarks = JsonConvert.DeserializeObject<Landmark[]>(_receiver.ReceivedMessage);
            retargetController.Retarget(landmarks);

            if (landmark)
                for (int i = 0; i < 33; i++)
                {
                    _debugLandmark[i].transform.position = new Vector3(landmarks[i].X, landmarks[i].Y, landmarks[i].Z);
                }
        }
    }
}