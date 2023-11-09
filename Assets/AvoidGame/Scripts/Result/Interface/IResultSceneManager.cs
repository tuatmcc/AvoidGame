using System.Collections.Generic;

namespace AvoidGame.Result.Interface
{
    public interface IResultSceneManager
    {
        public int PlayerRank { get; set; }
        public (List<long> timeList, long playerTime) GetTimeData();
    }
}