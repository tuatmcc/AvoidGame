using System;
using System.Threading;
using AvoidGame.Calibration.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.Misc
{
    public class RoomController : MonoBehaviour
    {
        [Inject] private ICalibrationStateManager _calibrationStateManager;
        [SerializeField] private Transform hatch1;
        [SerializeField] private Transform hatch2;
        [SerializeField] private Material hatchLight;
        private static readonly int EmissionMap = Shader.PropertyToID("_EmissionMap");

        private void Start()
        {
            hatchLight.SetColor(EmissionMap, Color.red);
            _calibrationStateManager.OnCalibrationStateChanged += (state) =>
            {
                if (state == CalibrationState.Transitioning)
                {
                    OpenHatch(this.GetCancellationTokenOnDestroy()).Forget();
                    ChangeHatchLight(this.GetCancellationTokenOnDestroy()).Forget();
                }
            };
        }

        private async UniTask OpenHatch(CancellationToken token)
        {
            var dx = 0.3f;
            await UniTask.Delay(1000, cancellationToken: token);
            while (dx < 10f && !token.IsCancellationRequested)
            {
                hatch1.localPosition += new Vector3(-dx, 0, 0);
                hatch2.localPosition += new Vector3(dx, 0, 0);
                await UniTask.Delay(10, cancellationToken: token);
            }
        }

        private async UniTask ChangeHatchLight(CancellationToken token)
        {
            var color = Color.red;
            await UniTask.Delay(2000, cancellationToken: token);
            while (color != Color.green && !token.IsCancellationRequested)
            {
                color = Color.Lerp(color, Color.green, 0.75f);
                hatchLight.SetColor(EmissionMap, color);
                await UniTask.Delay(10, cancellationToken: token);
            }
        }
    }
}