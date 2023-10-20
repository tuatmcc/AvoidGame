using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace DesignDemo
{
    /// <summary>
    /// DBにタイムを記録する
    /// </summary>
    public class DBRecorder : MonoBehaviour, ITimeRecordable
    {
        public event Action OnTimeRecorded;

        private void Start()
        {
        
        }

        public List<long> GetTimeRanking()
        {
            throw new System.NotImplementedException();
        }

        public void RecordTime(long time)
        {
            OnTimeRecorded?.Invoke();
        }

        // 非同期処理で保存することになる？
    }
}
