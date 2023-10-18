using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvoidGame
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
