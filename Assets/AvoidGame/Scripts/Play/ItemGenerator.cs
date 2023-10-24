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
            for(float i = 0f; i < path.MaxPos; i += 0.5f)
            {
                Debug.Log(path.EvaluateOrientation(i));
                var pos = path.EvaluateLocalPosition(i);
                pos.y += 0.3f;
                var rotation = path.EvaluateOrientation(i);
                var rot = Quaternion.AngleAxis(90.0f, Vector3.up);
                Instantiate(obj, pos, rot * rotation, this.transform);
            }
        }
    }
}
