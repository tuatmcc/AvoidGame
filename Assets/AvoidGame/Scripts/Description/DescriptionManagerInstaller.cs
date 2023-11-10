using Zenject;

namespace AvoidGame.Description
{
    public class DescriptionManagerInstaller : MonoInstaller<DescriptionManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(IDescriptionSceneManager), typeof(IInitializable))
                .To<DescriptionManager>()
                .AsSingle();
        }
    }
}