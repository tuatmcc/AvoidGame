using AvoidGame.Calibration.Interface;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AvoidGame.Calibration.UI
{
    public class CalibrationSceneUI : MonoBehaviour
    {
        [Inject] private ICalibrationStateManager _calibrationStateManager;
        [SerializeField] private RawImage[] glitchImage;

        private void Awake()
        {
            _calibrationStateManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
        }

        private void OnCalibrationStateChanged(CalibrationState state)
        {
            Debug.Log($"CalibrationState: {state}");
            if (state != CalibrationState.Finished) return;
            foreach (var image in glitchImage)
            {
                image.enabled = false;
            }
        }
    }
}