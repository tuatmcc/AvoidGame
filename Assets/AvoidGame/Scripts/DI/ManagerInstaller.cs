using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace AvoidGame.DI
{
    public class ManagerInstaller : MonoInstaller
    {
        [SerializeField] private TimeManager _timeManager;

        public override void InstallBindings()
        {
            Container.Bind(typeof(GameStateManager), typeof(IInitializable))
                .FromNew()
                .AsSingle();

            Container.Bind<TimeManager>()
                .FromInstance(_timeManager)
                .AsSingle();
        }
    }
}