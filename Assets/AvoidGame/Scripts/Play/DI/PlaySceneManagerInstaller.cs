using AvoidGame.Play;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.DI
{
    public class PlaySceneManagerInstaller : MonoInstaller
    {
        [SerializeField] PlaySceneManager playSceneManager;
        [SerializeField] SpeedManager speedManager;

        public override void InstallBindings()
        {
            Container.Bind<PlaySceneManager>()
                .FromInstance(playSceneManager)
                .AsSingle();

            Container.Bind<SpeedManager>()
                .FromInstance(speedManager)
                .AsSingle();
        }
    }
}
