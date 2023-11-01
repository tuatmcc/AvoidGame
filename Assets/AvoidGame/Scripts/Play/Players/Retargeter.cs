using AvoidGame.Calibration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Player
{
    /// <summary>
    /// リターゲットメソッドを呼び出す
    /// </summary>
    public class Retargeter : MonoBehaviour
    {
        [Inject] IMediaPipeManager _mediaPipeManager;
        
        [SerializeField] Calibrator calibrator;

        void Update()
        {
            calibrator.Retarget(_mediaPipeManager.LandmarkData);
        }
    }
}
