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

        private TPoseData _tPoseData = new TPoseData();
        private bool _retargetStarted = false;
        private bool _retargetFinished = false;
        private float _timeElapsed = 0f;


        private void Start()
        {
            _receiver = new Receiver();
            var token = this.GetCancellationTokenOnDestroy();
            _receiver.StartReceiver(token).Forget();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_retargetStarted)
            {
                _retargetStarted = true;
            }

            if (!_retargetStarted) return;

            _timeElapsed += Time.deltaTime;

            if (_timeElapsed <= 10f)
            {
                if (_receiver.ReceivedMessage != null)
                    _tPoseData.AccumulateLandmarks(
                        JsonConvert.DeserializeObject<Landmark[]>(_receiver.ReceivedMessage));
                return;
            }

            if (_timeElapsed > 10f && !_retargetFinished)
            {
                _retargetFinished = true;
                retargetController.CalcRetargetMultiplier(_tPoseData.GetAverageLandmarks());
                return;
            }

            var landmarks = JsonConvert.DeserializeObject<Landmark[]>(_receiver.ReceivedMessage);
            retargetController.Retarget(landmarks);
        }
    }
}