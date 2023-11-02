using System;

namespace AvoidGame.Calibration
{
    public interface ICalibrationStateHolder
    {
        public CalibrationState State { get; set; }
        public Action<CalibrationState> OnCalibrationStateChanged { get; }
    }
}