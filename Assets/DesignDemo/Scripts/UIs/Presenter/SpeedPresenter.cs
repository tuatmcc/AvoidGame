using TMPro;
using UnityEngine;
using Zenject;

namespace DesignDemo
{
    /// <summary>
    /// スコアのUIを更新する
    /// </summary>
    public class SpeedPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        [Inject] private SpeedManager _speedManager;

        private void Awake()
        {
            _speedManager.OnSpeedChanged += ChangeSpeedText;
        }

        private void ChangeSpeedText(float spped)
        {
            scoreText.text = $"{spped:0.0}";
        }

        private void Reset()
        {
            scoreText = GetComponent<TMP_Text>();
        }
    }
}