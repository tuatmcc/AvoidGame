using System;
using System.Linq;
using AvoidGame.MediaPipe;
using extOSC;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace AvoidGame
{
    /// <summary>
    /// MediaPipeの管理
    /// </summary>
    [RequireComponent(typeof(OSCReceiver))]
    public class MediaPipeManager : MonoBehaviour, IMediaPipeManager
    {
        [SerializeField] OSCReceiver receiver;

        public Landmark[] LandmarkData { get; private set; } =
            Enumerable.Range(0, 33).Select(_ => new Landmark()).ToArray();

        public bool IsReady { get; private set; } = false;

        private MediaPipeManager()
        {
            Debug.Log("MediaPipeManager Initialized, id: " + GetHashCode());
        }

        public void Awake()
        {
            Debug.Log($"MediaPipeManager Started: id: {GetHashCode()}");
            receiver.LocalPort = 8080;
            receiver.Bind("/pose", OnReceive);
        }

        private void OnReceive(OSCMessage oscMessage)
        {
            IsReady = true;
            var landmarks = oscMessage.Values[0].ArrayValue;
            for (var i = 0; i < 33; i++)
            {
                LandmarkData[i].X = landmarks[i].ArrayValue[0].FloatValue;
                LandmarkData[i].Y = landmarks[i].ArrayValue[1].FloatValue;
                LandmarkData[i].Z = landmarks[i].ArrayValue[2].FloatValue;
                LandmarkData[i].Visibility = landmarks[i].ArrayValue[3].FloatValue;
            }
        }
    }
}