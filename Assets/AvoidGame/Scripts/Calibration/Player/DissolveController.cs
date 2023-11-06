using System;
using UnityEngine;

namespace AvoidGame.Calibration.Player
{
    public class DissolveController : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

        private Material dissolveMaterial;
        private static readonly int Dissolve = Shader.PropertyToID("_Position");

        private void Start()
        {
            dissolveMaterial = skinnedMeshRenderer.material;
            dissolveMaterial.SetFloat(Dissolve, -2f);
        }

        private void Update()
        {
            dissolveMaterial.SetFloat(Dissolve, dissolveMaterial.GetFloat(Dissolve) + 0.01f);
        }
    }
}