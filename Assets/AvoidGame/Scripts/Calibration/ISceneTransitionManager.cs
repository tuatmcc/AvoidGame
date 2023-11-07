using System.Collections;
using UnityEngine.SceneManagement;

namespace AvoidGame.Calibration
{
    public interface ISceneTransitionManager
    {
        public void ForceExit();
        public void OnGameStateChanged(GameState gameState);
    }
}