using AvoidGame.Calibration.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AvoidGame.Calibration.UI
{
    public class CalibrationSceneUI : MonoBehaviour
    {
        [Inject] private ICalibrationStateManager _calibrationStateManager;
        [SerializeField] private TMP_Text scanDescription;
        [SerializeField] private TMP_Text playDescription;
        [SerializeField] private RawImage glitchImage;

        private void Awake()
        {
            _calibrationStateManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
            glitchImage.enabled = false;
            scanDescription.enabled = true;
            playDescription.enabled = false;
        }

        private void OnCalibrationStateChanged(CalibrationState state)
        {
            switch (state)
            {
                case CalibrationState.Waiting:
                {
                    scanDescription.enabled = true;
                    playDescription.enabled = false;
                    glitchImage.enabled = false;
                    break;
                }
                case CalibrationState.Calibrating:
                {
                    glitchImage.enabled = true;
                    scanDescription.enabled = false;
                    playDescription.enabled = false;
                    break;
                }
                case CalibrationState.Finished:
                {
                    playDescription.enabled = true;
                    glitchImage.enabled = false;
                    scanDescription.enabled = false;
                    break;
                }
                default:
                    scanDescription.enabled = false;
                    glitchImage.enabled = false;
                    playDescription.enabled = false;
                    break;
            }
        }
    }
}