using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Audio
{
    public class ItemSEManager : MonoBehaviour
    {
        [SerializeField] float SESpan = 0.05f;
        [SerializeField] AudioClip speedUp;
        [SerializeField] AudioClip speedDown;

        [Inject] PlaySceneManager sceneManager;
        [Inject] SpeedManager speedManager;
        [Inject] IAudioManager audioManager;

        private CancellationTokenSource cts;
        private float now_speed;

        void Start()
        {
            sceneManager.OnPlayStateChanged += PlayStarted;
            speedManager.OnSpeedChanged += GetChangedSpeed;
        }

        private void PlayStarted(PlaySceneState playSceneState)
        {
            if(playSceneState == PlaySceneState.Playing)
            {
                cts = new CancellationTokenSource();
                PlaySE(cts.Token).Forget();
            }
            if(playSceneState == PlaySceneState.Finished)
            {
                cts?.Cancel();
            }
        } 

        private void GetChangedSpeed(float speed)
        {
            now_speed = speed;
        }

        async UniTask PlaySE(CancellationToken token)
        {
            while (true)
            {
                var pre_speed = now_speed;
                await UniTask.Delay(TimeSpan.FromSeconds(SESpan), cancellationToken: token);
                if(pre_speed < now_speed)
                {
                    audioManager.PlaySe(speedUp);
                }
                else if(pre_speed > now_speed)
                {
                    audioManager.PlaySe(speedDown);
                }
            }
        }

        private void OnDisable()
        {
            cts?.Cancel();
        }
    }
}
