using UnityEngine;

namespace AvoidGame
{
    /// <summary>
    /// PlayerのPoseの倍率をScene間で共有するためのクラス
    /// </summary>
    public class PlayerInfo
    {
        public Vector3 BodyMultiplier { get; set; } = Vector3.one;
        public float FloorHeight { get; set; } = 0f;
    }
}