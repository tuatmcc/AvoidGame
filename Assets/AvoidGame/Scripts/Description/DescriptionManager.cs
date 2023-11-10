using System;
using AvoidGame.Interface;
using Zenject;

namespace AvoidGame.Description
{
    public class DescriptionManager : IDescriptionSceneManager, IInitializable
    {
        [Inject] private GameStateManager _gameStateManager;

        private DescriptionState _state = DescriptionState.CalibrationDescription;

        public DescriptionState State
        {
            get => _state;
            private set
            {
                OnStateChanged?.Invoke(value);
                _state = value;
            }
        }

        public void MoveToNext()
        {
            if (State == DescriptionState.ScoringDescription) _gameStateManager.MoveToNextState();
            else State++;
        }

        public event Action<DescriptionState> OnStateChanged;

        public void Initialize()
        {
        }
    }
}