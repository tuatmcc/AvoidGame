using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DesignDemo
{
    public class CSVRecorder : MonoBehaviour, ITimeRecordable
    {
        private string _csvPath = Application.dataPath + "/../record.csv";
        public event Action OnTimeRecorded;

        public List<long> GetTimeRanking()
        {
            if (!File.Exists(_csvPath))
                return new List<long>();
            return File.ReadLines(_csvPath).Select(v => long.Parse(v)).ToList();
        }

        public void RecordTime(long time)
        {
            File.AppendAllText(_csvPath, time + "\n");
            OnTimeRecorded?.Invoke();
        }
    }
}
