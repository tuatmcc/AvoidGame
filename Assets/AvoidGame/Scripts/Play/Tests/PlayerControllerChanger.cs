using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AvoidGame.Play.Player;
using Zenject;

namespace AvoidGame.Play.Test
{
    /// <summary>
    /// プレイヤーのコントロール元を決める
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class PlayerControllerChanger : MonoBehaviour
    {
        [SerializeField] DebugPlayerPoseChanger poseChanger;
        [SerializeField] Retargeter retargeter;

        [SerializeField] private bool useDebuger;

        [Inject] IMediaPipeManager _mediaPipeManager;

        private void Awake()
        {
            if(_mediaPipeManager.LandmarkData == null || useDebuger)
            {
                poseChanger.enabled = true;
                retargeter.enabled = false;
            } 
            else
            {
                retargeter.enabled = true;
                poseChanger.enabled = false;
            }
        }
    }
}
