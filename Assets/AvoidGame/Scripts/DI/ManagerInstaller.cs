using AvoidGame.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace AvoidGame.DI
{
    public class ManagerInstaller : MonoInstaller
    {
        [SerializeField] AudioManager audioManager;
        public override void InstallBindings()
        {
            Container.Bind(typeof(GameStateManager), typeof(IInitializable), typeof(IDisposable))
                .To<GameStateManager>()
                .FromNew()
                .AsSingle();

            Container.Bind<IAudioManager>()
                .To<AudioManager>()
                .FromInstance(audioManager);
        }
    }
}