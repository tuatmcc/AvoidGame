using AvoidGame.Play;
using System;
using UnityEngine;
using Zenject;

namespace AvoidGame.DI
{
    public class PlaySceneManagerInstaller : MonoInstaller
    {
        [SerializeField] PlaySceneManager playSceneManager;

        public override void InstallBindings()
        {
            Container.Bind<PlaySceneManager>()
                .FromInstance(playSceneManager)
                .AsSingle();

            Container.Bind(typeof(SpeedManager), typeof(IInitializable))
                .To<SpeedManager>()
                .FromNew()
                .AsSingle();
        }
    }
}
