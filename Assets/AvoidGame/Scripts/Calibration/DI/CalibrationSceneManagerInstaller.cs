using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.DI
{
    public class CalibrationSceneManagerInstaller : MonoInstaller
    {
        [SerializeField] private CalibrationSceneManager calibrationSceneManager;

        public override void InstallBindings()
        {
            Container.Bind<CalibrationSceneManager>()
                .FromInstance(calibrationSceneManager)
                .AsSingle();
        }
    }
}