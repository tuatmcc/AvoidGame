using System;

namespace AvoidGame.Calibration.Interface
{
    public interface ICalibrationStateManager
    {
        public float WaitingDuration { get; }
        public float CalibratingDuration { get; }
        public float DissolvingDuration { get; }
        public float FinishedDuration { get; }
        public CalibrationState State { get; set; }
        public Action<CalibrationState> OnCalibrationStateChanged { get; set; }
    }
}