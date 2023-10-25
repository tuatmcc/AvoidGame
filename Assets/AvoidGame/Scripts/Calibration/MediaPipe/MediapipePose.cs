using UnityEngine;

namespace AvoidGame.Calibration.MediaPipe
{
    public class MediaPipePose
    {
        public Landmark[] landmarks = new Landmark[33];

        public static Vector3 GetPosition(Landmark landmark)
        {
            return new Vector3(landmark.X, landmark.Y, landmark.Z);
        }
    }
}