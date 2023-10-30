using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace AvoidGame.DI
{
    public class ManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameStateManager _gameStateManager;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private MediaPipeManager mediaPipeManager;

        public override void InstallBindings()
        {
            Container.Bind<GameStateManager>()
                .FromInstance(_gameStateManager)
                .AsSingle();

            Container.Bind<TimeManager>()
                .FromInstance(_timeManager)
                .AsSingle();

            Container.Bind<IMediaPipeManager>().To<MediaPipeManager>()
                .FromInstance(mediaPipeManager)
                .AsSingle();
        }
    }
}