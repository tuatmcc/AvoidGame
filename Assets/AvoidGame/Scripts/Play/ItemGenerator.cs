using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvoidGame.Play
{
    public class ItemGenerator : MonoBehaviour
    {
        [SerializeField] Cinemachine.CinemachinePath path;
        [SerializeField] GameObject obj;

        void Start()
        {
            for(float i = 0f; i < path.MaxPos; i += 0.1f)
            {
                Debug.Log(path.EvaluateLocalPosition(i));
                Instantiate(obj, path.EvaluateLocalPosition(i), Quaternion.identity, this.transform);
            }
        }

        void Update()
        {
        
        }
    }
}
