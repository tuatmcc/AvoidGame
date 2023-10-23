using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Test.UI 
{
    public class TimePresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [Inject] TimeManager _timeManager;

        private void Start()
        {
            text.SetText($"Time : 00:00:000");
            _timeManager.OnTimeChanged += ChangeText;
        }

        private void ChangeText(TimeSpan time)
        {
            text.SetText($"Time : {time.Minutes:00}:{time.Seconds:00}:{time.Milliseconds:000}");
        }
    }
}
