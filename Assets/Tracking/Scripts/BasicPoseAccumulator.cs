using UnityEngine;
using Tracking.MediaPipe;

namespace Tracking
{
    public class BasicPoseAccumulator
    {
        private readonly AccumulatedLandmark[] _accumulatedLandmarks = new AccumulatedLandmark[33];

        public BasicPoseAccumulator()
        {
            for (var i = 0; i < _accumulatedLandmarks.Length; i++)
            {
                _accumulatedLandmarks[i] = new AccumulatedLandmark();
            }
        }

        public void AccumulateLandmarks(Landmark[] landmarks)
        {
            if (landmarks.Length != 33) return;
            for (var i = 0; i < landmarks.Length; i++)
            {
                _accumulatedLandmarks[i].Add(landmarks[i]);
            }
        }

        public Landmark[] GetAverageLandmarks()
        {
            var averageLandmarks = new Landmark[33];
            for (var i = 0; i < _accumulatedLandmarks.Length; i++)
            {
                averageLandmarks[i] = _accumulatedLandmarks[i].GetAverage();
            }

            return averageLandmarks;
        }
    }
}