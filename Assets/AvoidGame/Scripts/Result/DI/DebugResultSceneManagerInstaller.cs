using AvoidGame.Result.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result.DI
{
    public class DebugResultSceneManagerInstaller : MonoInstaller<DebugResultSceneManagerInstaller>
    {
        [SerializeField] private DebugResultSceneManager debugSceneManager;

        public override void InstallBindings()
        {
            Container.Bind(typeof(IResultSceneManager), typeof(IInitializable))
                .To<DebugResultSceneManager>()
                .FromInstance(debugSceneManager)
                .AsCached();
        }
    }
}