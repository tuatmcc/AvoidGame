using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パス上にアイテムを生成する
/// </summary>
namespace AvoidGame.Play
{
    public class ItemGenerator : MonoBehaviour
    {
        [SerializeField] Cinemachine.CinemachinePath path;
        [SerializeField] List<GameObject> objs;
        [SerializeField, Range(0.0f, 5.0f)] List<float> itemPosition;

        void Start()
        {
            foreach(var position in itemPosition)
            {
                var pos = path.EvaluateLocalPosition(position);
                var rotation = path.EvaluateOrientation(position);
                var rot = Quaternion.AngleAxis(90.0f, Vector3.up);
                Instantiate(objs[Random.Range(0, objs.Count)], pos, rot * rotation, this.transform);
            }
        }
    }
}
