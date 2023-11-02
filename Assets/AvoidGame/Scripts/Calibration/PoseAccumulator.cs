using System.Linq;
using AvoidGame.MediaPipe;

namespace AvoidGame.Calibration
{
    /// <summary>
    /// Accumulates T Pose landmark data for calibration
    /// </summary>
    public class PoseAccumulator
    {
        private readonly Landmark[] _accumulatedLandmarks = Enumerable.Repeat(new Landmark(), 33).ToArray();
        private int _accumulatedCount = 0;

        public void AccumulateLandmarks(Landmark[] landmarks)
        {
            for (var i = 0; i < landmarks.Length; i++)
            {
                _accumulatedLandmarks[i].X += landmarks[i].X;
                _accumulatedLandmarks[i].Y += landmarks[i].Y;
                _accumulatedLandmarks[i].Z += landmarks[i].Z;
                _accumulatedLandmarks[i].Visibility += landmarks[i].Visibility;
            }

            _accumulatedCount++;
        }

        public Landmark[] GetAverageLandmarks()
        {
            var averageLandmarks = new Landmark[_accumulatedLandmarks.Length];
            for (var i = 0; i < _accumulatedLandmarks.Length; i++)
            {
                averageLandmarks[i] = new Landmark
                {
                    X = _accumulatedLandmarks[i].X / _accumulatedCount,
                    Y = _accumulatedLandmarks[i].Y / _accumulatedCount,
                    Z = _accumulatedLandmarks[i].Z / _accumulatedCount,
                    Visibility = _accumulatedLandmarks[i].Visibility / _accumulatedCount
                };
            }

            return averageLandmarks;
        }
    }
}