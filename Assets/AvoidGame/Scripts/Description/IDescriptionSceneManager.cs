using System;

namespace AvoidGame.Description
{
    public interface IDescriptionSceneManager
    {
        public DescriptionState State { get; }

        public void MoveToNext();
        
        public event Action<DescriptionState> OnStateChanged; 
    }
}