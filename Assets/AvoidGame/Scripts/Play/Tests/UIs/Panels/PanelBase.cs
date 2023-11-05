using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Tests.UIs.Panels
{
    /// <summary>
    /// パネルの基底クラス
    /// GameStateに応じたUIの表示・非表示を行う
    /// TargetStateに指定されたGameStateのときにのみ表示される
    /// </summary>
    public abstract class PanelBase : MonoBehaviour
    {
        public virtual PlaySceneState TargetState { get; }

        [Inject] private PlaySceneManager _playSceneManager;

        private void Awake()
        {
            _playSceneManager.OnPlayStateChanged += ChangePanelActivation;
        }

        private void ChangePanelActivation(PlaySceneState state)
        {
            gameObject.SetActive(state == TargetState);
        }

        private void OnDisable()
        {
            _playSceneManager.OnPlayStateChanged -= ChangePanelActivation;
        }
    }
}