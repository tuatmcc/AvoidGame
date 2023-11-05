using UnityEngine;

namespace AvoidGame
{
    /// <summary>
    /// Playerに関する情報をScene間で共有するためのクラス
    /// </summary>
    public class PlayerInfo
    {
        public Vector3 BodyMultiplier { get; set; } = Vector3.one;
        public float FloorHeight { get; set; } = 0f;
        public long Time { get; set; } = 0;
    }
}