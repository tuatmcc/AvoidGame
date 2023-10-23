using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Player
{
    /// <summary>
    /// プレイヤー
    /// アイテムの取得を行う
    /// </summary>
    public class Player : MonoBehaviour
    {
        private float default_speed;
        private Cinemachine.CinemachineDollyCart cart;
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

        private void StartRace(GameState gameState)
        {
            if(gameState == GameState.Playing)
            {
                cart.m_Speed = default_speed;
            }
        }

        private void ChangeSpeed(float speed)
        {
            cart.m_Speed = default_speed * speed;
        }
    }
}
