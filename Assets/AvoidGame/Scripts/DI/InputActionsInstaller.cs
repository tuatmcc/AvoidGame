using Zenject;

namespace AvoidGame.DI
{
    public class InputActionsInstaller : MonoInstaller<InputActionsInstaller>
    {
        private AvoidGameInputActions _inputActions;

        public override void InstallBindings()
        {
            _inputActions = new AvoidGameInputActions();
            Container.BindInstance(_inputActions).AsSingle();
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
            _inputActions.Dispose();
        }
    }
}