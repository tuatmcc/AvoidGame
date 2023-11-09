using System;
using UnityEngine;

namespace AvoidGame.Result
{
    public class ResultObjectActivationController : MonoBehaviour
    {
        [SerializeField] private ResultCutManager resultCutManager;

        private void Start()
        {
            resultCutManager.GetCurrentPattern().activationObject.SetActive(true);
        }
    }
}