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

        public void Awake()
        {
            receiver.LocalPort = 8080;
            receiver.Bind("/pose", OnReceive);
        }

        private void OnReceive(OSCMessage oscMessage)
        {
            IsReady = true;
            var landmarks = oscMessage.Values[0].ArrayValue;
            for (var i = 0; i < 33; i++)
            {
                // original: 0 < x < 1 (left -> right?), after: -0.5 < x 0.5 (right -> left?)
                LandmarkData[i].X = 0.5f -landmarks[i].ArrayValue[0].FloatValue;
                // original: 0 < y < 1 (top -> down), after: 0 < y < 1 (down -> top)
                LandmarkData[i].Y = 1-landmarks[i].ArrayValue[1].FloatValue; 
                LandmarkData[i].Z = -landmarks[i].ArrayValue[2].FloatValue;
                LandmarkData[i].Visibility = landmarks[i].ArrayValue[3].FloatValue;
            }
        }
    }
}