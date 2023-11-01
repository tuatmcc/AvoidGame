using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AvoidGame.Calibration
{
    public class CalibrationSceneUI : MonoBehaviour
    {
        [Inject] private CalibrationSceneManager _calibrationSceneManager;
        [SerializeField] private RawImage[] glitchImage;

        private void Awake()
        {
            _calibrationSceneManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
        }

        private void OnCalibrationStateChanged(CalibrationSceneManager.CalibrationState state)
        {
            if (state != CalibrationSceneManager.CalibrationState.Finished) return;
            foreach (var image in glitchImage)
            {
                image.enabled = false;
            }
        }
    }
}