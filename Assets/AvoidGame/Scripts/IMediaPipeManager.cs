using System.Net.Sockets;
using AvoidGame.MediaPipe;

namespace AvoidGame
{
    public interface IMediaPipeManager
    {
        public Landmark[] LandmarkData { get; }
        public bool IsReady { get; }
    }
}