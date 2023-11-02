using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Test.UI
{
    public class SpeedPresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [Inject] SpeedManager _speedManager;

        private void Start()
        {
            text.SetText("Speed : 1.0");
            _speedManager.OnSpeedChanged += ChangeText;
        }

        private void ChangeText(float speed)
        {
            text.SetText($"Speed : {speed:0.0}");
        }
    }
}
