using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using AvoidGame.TimeRecorder;

namespace AvoidGame.DI
{
    public class TimeRecorderInstaller : MonoInstaller
    {
        [SerializeField] private CSVRecorder _recorder;

        public override void InstallBindings()
        {
            Container.Bind<ITimeRecordable>()
                .To<CSVRecorder>()
                .FromInstance(_recorder)
                .AsSingle();
        }
    }
}
