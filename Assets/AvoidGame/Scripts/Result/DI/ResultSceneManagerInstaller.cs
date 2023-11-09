using AvoidGame.Result.Interface;
using Zenject;

namespace AvoidGame.Result.DI
{
    public class ResultSceneManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(IResultSceneManager), typeof(IInitializable))
                .To<ResultSceneManager>()
                .AsSingle();
        }
    }
}