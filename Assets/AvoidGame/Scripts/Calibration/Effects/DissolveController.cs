using System;
using AvoidGame.Calibration.Interface;
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

        private float _currentDissolvePosition = -2f;

        private void Awake()
        {
            _dissolveMaterial = skinnedMeshRenderer.material;
            _calibrationStateManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
            _dissolveMaterial.SetFloat(Dissolve, _currentDissolvePosition);
        }


        private void OnCalibrationStateChanged(CalibrationState state)
        {
            if (state != CalibrationState.Calibrating) return;

            _currentDissolvePosition = -2f;
            _dissolveMaterial.SetFloat(Dissolve, _currentDissolvePosition);
        }

        private void Update()
        {
            switch (_calibrationStateManager.State)
            {
                case CalibrationState.Finishing:
                {
                    _currentDissolvePosition += Time.deltaTime;
                    _dissolveMaterial.SetFloat(Dissolve, _currentDissolvePosition);
                    break;
                }
                case CalibrationState.Finished:
                {
                    _dissolveMaterial.SetFloat(Dissolve, 1f);
                    break;
                }
            }
        }
    }
}