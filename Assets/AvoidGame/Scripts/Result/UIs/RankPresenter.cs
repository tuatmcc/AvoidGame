using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result.UI
{
    public class RankPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text rankText;
        [SerializeField] private int targetRank;
        [Inject] private ResultSceneManager _sceneManager;
        void Start()
        {
            rankText = GetComponent<TMP_Text>();
            var _data = _sceneManager.GetTimeData();
            SetRankText(_data.timeList, _data.playerTime);
        }

        private void SetRankText(List<long> timeList, long time)
        {
            targetRank = (targetRank == 0 ? timeList.IndexOf(time) + 1 : targetRank);
            rankText.text = $"{targetRank}位";
        }
    }
}
