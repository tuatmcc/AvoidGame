using AvoidGame.Play;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Test.UI
{
    public class CountDownPresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [Inject] PlaySceneManager _playSceneManager;

        private void Start()
        {
            text.SetText("");
            _playSceneManager.OnCountChanged += ChangeText;
        }

        private void ChangeText(int count)
        {
            text.SetText($"CountDown : {count}");
        }
    }
}
