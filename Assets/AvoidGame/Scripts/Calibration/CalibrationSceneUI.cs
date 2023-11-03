using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AvoidGame.Calibration
{
    public class CalibrationSceneUI : MonoBehaviour
    {
        [Inject] private ICalibrationStateHolder _calibrationStateHolder;
        [SerializeField] private RawImage[] glitchImage;

        private void Awake()
        {
            _calibrationStateHolder.OnCalibrationStateChanged += OnCalibrationStateChanged;
        }

        private void OnCalibrationStateChanged(CalibrationState state)
        {
            if (state != CalibrationState.Finished) return;
            foreach (var image in glitchImage)
            {
                image.enabled = false;
            }
        }
    }
}