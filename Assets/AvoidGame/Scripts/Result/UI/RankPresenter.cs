using AvoidGame.Result.Interface;
using TMPro;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result.UI
{
    public class RankPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text rankText;
        [SerializeField] private int targetRank;
        [Inject] private IResultSceneManager _sceneManager;

        void Start()
        {
            rankText = GetComponent<TMP_Text>();
            SetRankText();
        }

        private void SetRankText()
        {
            targetRank = (targetRank == 0 ? _sceneManager.PlayerRank : targetRank);
            rankText.text = $"{targetRank}位";
        }
    }
}