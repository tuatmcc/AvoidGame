using UnityEngine;

namespace Tracking
{
    public class Body
    {
        const int LANDMARK_COUNT = 33;
        const int LINES_COUNT = 11;

        public Transform parent;
        public AccumulatedBuffer[] positionsBuffer = new AccumulatedBuffer[LANDMARK_COUNT];
        public Vector3[] localPositionTargets = new Vector3[LANDMARK_COUNT];
        public GameObject[] instances = new GameObject[LANDMARK_COUNT];

        public bool active;

        public Body(Transform parent, GameObject landmarkPrefab, float s,
            GameObject headPrefab)
        {
            this.parent = parent;
            for (int i = 0; i < instances.Length; ++i)
            {
                instances[i] =
                    Object.Instantiate(landmarkPrefab); // GameObject.CreatePrimitive(PrimitiveType.Sphere);
                instances[i].transform.localScale = Vector3.one * s;
                instances[i].transform.parent = parent;
                instances[i].name = ((LandmarkIndex)i).ToString();
            }

            if (headPrefab)
            {
                GameObject head = Object.Instantiate(headPrefab);
                head.transform.parent = instances[(int)LandmarkIndex.NOSE].transform;
                head.transform.localPosition = headPrefab.transform.position;
                head.transform.localRotation = headPrefab.transform.localRotation;
                head.transform.localScale = headPrefab.transform.localScale;
            }
        }
    }
}