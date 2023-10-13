using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Result.ModelController
{
    public class DirectionController : MonoBehaviour
    {
        void Start()
        {
            Vector3 p = Camera.main.transform.position;
            p.y = transform.position.y;
            transform.LookAt(p);
        }
    }
}
