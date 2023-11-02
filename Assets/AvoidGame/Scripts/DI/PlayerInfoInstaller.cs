using UnityEngine;
using Zenject;

namespace AvoidGame.DI
{
    public class PlayerInfoInstaller : MonoInstaller
    {
        private readonly PlayerInfo _playerInfo = new();

        public override void InstallBindings()
        {
            Container.Bind<PlayerInfo>()
                .FromInstance(_playerInfo);
        }
    }
}