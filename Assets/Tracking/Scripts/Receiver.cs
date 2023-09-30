using System.Threading;
using Cysharp.Threading.Tasks;
using System.Net.Sockets;

namespace Tracking
{
    public class Receiver
    {
        private UdpClient _udpClient;

        public string ReceivedMessage { get; private set; } = null;

        public Receiver()
        {
            _udpClient = new UdpClient(5000);
        }

        public async UniTask StartReceiver(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var result = await _udpClient.ReceiveAsync();
                var data = result.Buffer;
                ReceivedMessage = System.Text.Encoding.UTF8.GetString(data);
            }
        }
    }
}