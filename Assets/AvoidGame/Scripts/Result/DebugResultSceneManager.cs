using System;
using System.Collections.Generic;
using AvoidGame.Result.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result
{
    public class DebugResultSceneManager : MonoBehaviour, IResultSceneManager, IInitializable
    {
        [SerializeField] private int playerRank = 0;

        public int PlayerRank
        {
            get => playerRank;
            set => playerRank = value;
        }

        public void Initialize()
        {
        }

        public (List<long> timeList, long playerTime) GetTimeData()
        {
            return (new List<long>(), 1000000);
        }
    }
}