using AvoidGame.Result.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result.DI
{
    public class ResultSceneManagerInstaller : MonoInstaller
    {
        [SerializeField] private bool debug = false;
        [SerializeField] private DebugResultSceneManager debugResultSceneManager;

        public override void InstallBindings()
        {
            if (debug)
            {
                Container.Bind(typeof(IResultSceneManager), typeof(IInitializable))
                    .To<DebugResultSceneManager>()
                    .FromInstance(debugResultSceneManager)
                    .AsSingle();
                return;
            }

            Container.Bind(typeof(IResultSceneManager), typeof(IInitializable))
                .To<ResultSceneManager>()
                .AsSingle();
        }
    }
}