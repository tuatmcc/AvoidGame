using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result.DI
{
    public class ResultSceneManagerInstaller : MonoInstaller
    {
        [SerializeField] private ResultSceneManager _resultSceneManager;

        public override void InstallBindings()
        {
            Container.Bind<ResultSceneManager>()
                .FromInstance(_resultSceneManager)
                .AsSingle();
        }
    }
}
