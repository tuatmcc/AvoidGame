using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using AvoidGame.MediaPipe;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace AvoidGame
{
    /// <summary>
    /// MediaPipeの管理
    /// </summary>
    public class MediaPipeManager : MonoBehaviour, IMediaPipeManager
    {
        private readonly Receiver _receiver = new();
        public Landmark[] LandmarkData { get; private set; } = Enumerable.Repeat(new Landmark(), 33).ToArray();

        public bool IsReady { get; private set; } = false;

        private void Start()
        {
            _receiver.OnReceive += OnReceive;
            Debug.Log("MediaPipeManager Initialized");
            var token = this.GetCancellationTokenOnDestroy();
            _receiver.StartReceiver(token).Forget();
        }

        private void OnReceive(UdpReceiveResult result)
        {
            IsReady = true;
            Debug.Log($"Received. IsReady: {IsReady}");
            try
            {
                var json = Encoding.UTF8.GetString(result.Buffer);
                LandmarkData = JsonConvert.DeserializeObject<Landmark[]>(json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}