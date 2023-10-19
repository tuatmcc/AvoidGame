using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.DI
{
    public class ManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameStateManager _gameStateManager;
        [SerializeField] private TimeManager _timeManager;

        public override void InstallBindings()
        {
            Container.Bind<GameStateManager>()
                .FromInstance(_gameStateManager)
                .AsSingle();

            Container.Bind<TimeManager>()
                .FromInstance(_timeManager)
                .AsSingle();
        }
    }
}

