using System;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.DI
{
    public class TimeManagerInstaller : MonoInstaller<TimeManagerInstaller>
    {
        [SerializeField] private TimeManager timeManager;

        public override void InstallBindings()
        {
            Container.BindInstance(timeManager).AsSingle();
        }
    }
}