using UnityEngine;
using Zenject;

namespace AvoidGame.DI
{
    public class PlayerInfoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInfo>()
                .FromNew()
                .AsSingle();
        }
    }
}