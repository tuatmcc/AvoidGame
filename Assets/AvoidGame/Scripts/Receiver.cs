using System;
using System.Net.Sockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AvoidGame
{
    /// <summary>
    /// UDP Receiver for MediaPipe
    /// </summary>
    public class Receiver
    {
        private readonly UdpClient _udpClient;
        public event Action<UdpReceiveResult> OnReceive;

        /// <summary>
        /// Initialize Receiver
        /// </summary>
        /// <param name="port"> Port number to receive </param>
        public Receiver(int port = 8080)
        {
            Debug.Log("Called Reciever");
            _udpClient = new UdpClient(port);
        }

        public async UniTask StartReceiver(CancellationToken token)
        {
            Debug.Log("UDP Receiver Started");
            while (!token.IsCancellationRequested)
            {
                // wait for receive (blocking)
                var result = await _udpClient.ReceiveAsync();
                OnReceive?.Invoke(result);
            }
        }

        public void CloseCliant()
        {
            try
            {
                _udpClient.Close();
            }catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }

}