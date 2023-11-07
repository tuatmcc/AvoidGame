using System;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.DI
{
    public class PlaySceneManagerInstaller : MonoInstaller
    {
        [SerializeField] PlaySceneManager playSceneManager;

        public override void InstallBindings()
        {
            Container.Bind<PlaySceneManager>()
                .FromInstance(playSceneManager)
                .AsSingle();

            Container.Bind(typeof(SpeedManager), typeof(IInitializable), typeof(IDisposable))
                .To<SpeedManager>()
                .FromNew()
                .AsSingle();
        }
    }
}
