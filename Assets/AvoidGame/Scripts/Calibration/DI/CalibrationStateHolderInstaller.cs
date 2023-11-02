using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.DI
{
    public class CalibrationStateHolderInstaller : MonoInstaller<CalibrationStateHolderInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ICalibrationStateHolder>()
                .To<CalibrationStateHolder>()
                .AsSingle();
        }
    }
}