using AvoidGame.Calibration.Interface;
using System;
using Zenject;

namespace AvoidGame.Calibration.DI
{
    public class CalibrationStateManagerInstaller : MonoInstaller<CalibrationStateManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(ICalibrationStateManager), typeof(IInitializable), typeof(IDisposable))
                .To<CalibrationStateManager>()
                .AsSingle();
        }
    }
}