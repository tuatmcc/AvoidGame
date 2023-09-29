using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム中のスピードを管理する
/// </summary>
public class SpeedManager : MonoBehaviour
{
    /// <summary>
    /// スピード倍率
    /// </summary>
    public float Speed 
    {
        get => Speed;
        set { }
    }

    /// <summary>
    /// スピードを変更する処理
    /// </summary>
    /// <param name="add"></param>
    public void AddPlayerSpeed(float add)
    {
        Speed += add;
    }
}
