using System.Collections.Generic;
using Tracking.MediaPipe;

namespace AvoidGame
{
    /// <summary>
    /// Values of a landmark accumulated over time during calibration
    /// </summary>
    public class AccumulatedLandmark
    {
        private readonly List<Landmark> _data = new List<Landmark>();

        public void Add(Landmark landmark)
        {
            _data.Add(landmark);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public Landmark GetAverage()
        {
            var average = new Landmark();
            foreach (var landmark in _data)
            {
                average.X += landmark.X;
                average.Y += landmark.Y;
                average.Z += landmark.Z;
            }

            average.X /= _data.Count;
            average.Y /= _data.Count;
            average.Z /= _data.Count;

            return average;
        }
    }
}
