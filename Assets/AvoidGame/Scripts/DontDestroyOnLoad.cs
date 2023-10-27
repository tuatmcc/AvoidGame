using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DontDestroyOnLoad送りにする
/// </summary>
namespace AvoidGame
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
