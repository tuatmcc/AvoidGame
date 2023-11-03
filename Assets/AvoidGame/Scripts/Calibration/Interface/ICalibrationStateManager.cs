using System;

namespace AvoidGame.Calibration.Interface
{
    public interface ICalibrationStateManager
    {
        public CalibrationState State { get; set; }
        public Action<CalibrationState> OnCalibrationStateChanged { get; set; }
    }
}