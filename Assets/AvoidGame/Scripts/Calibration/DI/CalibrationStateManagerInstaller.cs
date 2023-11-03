using AvoidGame.Calibration.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.DI
{
    public class CalibrationStateManagerInstaller : MonoInstaller<CalibrationStateManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(ICalibrationStateManager), typeof(IInitializable))
                .To<CalibrationStateManager>()
                .AsSingle();
        }
    }
}