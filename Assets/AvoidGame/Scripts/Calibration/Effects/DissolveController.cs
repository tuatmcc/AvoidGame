using System;
using System.Threading;
using AvoidGame.Calibration.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.Effects
{
    public class DissolveController : MonoBehaviour
    {
        [Inject] private ICalibrationStateManager _calibrationStateManager;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

        private Material _dissolveMaterial;
        private static readonly int Dissolve = Shader.PropertyToID("_Position");

        private readonly float _startDissolvePosition = -2f;

        private void Awake()
        {
            _dissolveMaterial = skinnedMeshRenderer.material;
            _calibrationStateManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
            _dissolveMaterial.SetFloat(Dissolve, _startDissolvePosition);
        }


        private void OnCalibrationStateChanged(CalibrationState state)
        {
            if (state == CalibrationState.Dissolving)
            {
                ChangeDissolvePosition(this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        private async UniTask ChangeDissolvePosition(CancellationToken token)
        {
            var currentDissolvePosition = -2f;
            while (currentDissolvePosition < 1f && !token.IsCancellationRequested)
            {
                currentDissolvePosition = Mathf.Lerp(currentDissolvePosition, 1f, 0.05f);
                _dissolveMaterial.SetFloat(Dissolve, currentDissolvePosition);
                await UniTask.Delay(10, cancellationToken: token);
            }
        }
    }
}