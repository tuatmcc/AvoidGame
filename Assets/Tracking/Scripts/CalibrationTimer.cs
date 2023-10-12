using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Tracking
{
    public class CalibrationTimer : MonoBehaviour
    {
        [SerializeField] private Avatar avatar;
        public Server server;
        public int timer = 5;
        public KeyCode calibrationKey = KeyCode.C;

        private bool calibrated;

        private void Start()
        {
            Debug.Log("Press " + calibrationKey + " to start calibration timer.");
        }

        private void Update()
        {
            if (Input.GetKeyDown(calibrationKey))
            {
                if (!calibrated)
                {
                    calibrated = true;
                    var token = this.GetCancellationTokenOnDestroy();
                    Timer(token).Forget();
                }
                else
                {
                    Notify().Forget();
                }
            }
        }

        private async UniTask Timer(CancellationToken token)
        {
            int t = timer;
            while (t > 0 && !token.IsCancellationRequested)
            {
                Debug.Log("Copy the avatars starting pose: " + t.ToString());
                await UniTask.WaitForSeconds(1f, cancellationToken: token);
                --t;
            }

            avatar.Calibrate();
            Debug.Log("Calibration Completed");
            server.SetVisible(false);

            await UniTask.WaitForSeconds(1.5f, cancellationToken: token);
        }

        private async UniTask Notify()
        {
            Debug.Log("Must restart instance to recalibrate.");
            await UniTask.WaitForSeconds(3f);
        }
    }
}