using AvoidGame.Play.Player;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Players
{
    /// <summary>
    /// プレイヤー
    /// アイテムの取得を行う
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] Cinemachine.CinemachinePath path;
        private float default_speed;
        private Cinemachine.CinemachineDollyCart cart;
        [Inject] PlaySceneManager _playSceneManager;
        [Inject] SpeedManager _speedManager;
        [Inject] GameStateManager _gameStateManager;

        private void Start()
        {
            default_speed = PlayerConstants.default_player_speed;
            cart = GetComponent<Cinemachine.CinemachineDollyCart>();
            cart.m_Speed = 0f;

            _gameStateManager.OnGameStateChanged += StartRace;
            _speedManager.OnSpeedChanged += ChangeSpeed;
        }

        /// <summary>\
        /// 終点に達したらManagerに通知
        /// </summary>
        private void Update()
        {
            if (cart.m_Position == path.PathLength && _playSceneManager.State == PlaySceneState.Playing)
            {
                _playSceneManager.Finished();
            }
        }

        private void StartRace(GameState gameState)
        {
            if (_playSceneManager.State == PlaySceneState.Playing)
            {
                cart.m_Speed = default_speed;
            }
        }

        /// <summary>
        /// スピードの変更をcartに反映
        /// </summary>
        /// <param name="speed"></param>
        private void ChangeSpeed(float speed)
        {
            cart.m_Speed = speed;
        }

        private void OnDisable()
        {
            _gameStateManager.OnGameStateChanged -= StartRace;
        }
    }
}