using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvoidGame.System.TimeRecorder
{
    /// <summary>
    /// タイムを記録、取得できる
    /// </summary>
    public interface ITimeRecordable
    {
        /// <summary>
        /// UI系がタイムのレコード記録完了後に情報を取得
        /// </summary>
        public event Action OnTimeRecorded;

        /// <summary>
        /// タイムを記録する
        /// </summary>
        /// <param name="time"></param>
        public void RecordTime(long time);

        /// <summary>
        /// タイム一覧を取得する
        /// </summary>
        /// <returns></returns>
        public List<long> GetTimeRanking();
    }
}