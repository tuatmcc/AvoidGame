using UnityEngine;
using Zenject;

namespace AvoidGame.DI
{
    public class MediaPipeManagerInstaller : MonoInstaller<MediaPipeManagerInstaller>
    {
        [SerializeField] private MediaPipeManager mediaPipeManager;

        public override void InstallBindings()
        {
            Container.Bind<IMediaPipeManager>()
                .To<MediaPipeManager>()
                .FromInstance(mediaPipeManager)
                .AsSingle();
        }
    }
}