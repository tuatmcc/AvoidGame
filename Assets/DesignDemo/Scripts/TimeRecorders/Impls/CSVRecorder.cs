using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVRecorder : MonoBehaviour, ITimeRecordable
{
    private string _csvPath = Application.dataPath + "/../record/csv";
    public event Action OnTimeRecorded;
    
    public List<float> GetTimeRanking(int num)
    {
        if (!File.Exists(_csvPath))
            return new List<float>();
        return File.ReadLines(_csvPath).SkipLast(1).Select(v => float.Parse(v)).ToList();
    }

    public void RecordTime(float time)
    {
        File.AppendAllText(_csvPath, time + "\n");
    }
}
