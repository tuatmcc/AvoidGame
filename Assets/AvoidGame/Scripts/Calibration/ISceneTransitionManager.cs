using System.Collections;
using UnityEngine.SceneManagement;

namespace AvoidGame.Calibration
{
    public interface ISceneTransitionManager
    {
        public IEnumerator LoadScene(string sceneName);
        public void RegisterEvents();
    }
}