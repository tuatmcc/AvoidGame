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
        public LineRenderer[] lines = new LineRenderer[LINES_COUNT];

        public bool active;

        public Body(Transform parent, GameObject landmarkPrefab, GameObject linePrefab, float s,
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

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = Object.Instantiate(linePrefab).GetComponent<LineRenderer>();
                lines[i].transform.parent = parent;
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

        public void UpdateLines()
        {
            lines[0].positionCount = 4;
            lines[0].SetPosition(0, Position((LandmarkIndex)32));
            lines[0].SetPosition(1, Position((LandmarkIndex)30));
            lines[0].SetPosition(2, Position((LandmarkIndex)28));
            lines[0].SetPosition(3, Position((LandmarkIndex)32));
            lines[1].positionCount = 4;
            lines[1].SetPosition(0, Position((LandmarkIndex)31));
            lines[1].SetPosition(1, Position((LandmarkIndex)29));
            lines[1].SetPosition(2, Position((LandmarkIndex)27));
            lines[1].SetPosition(3, Position((LandmarkIndex)31));

            lines[2].positionCount = 3;
            lines[2].SetPosition(0, Position((LandmarkIndex)28));
            lines[2].SetPosition(1, Position((LandmarkIndex)26));
            lines[2].SetPosition(2, Position((LandmarkIndex)24));
            lines[3].positionCount = 3;
            lines[3].SetPosition(0, Position((LandmarkIndex)27));
            lines[3].SetPosition(1, Position((LandmarkIndex)25));
            lines[3].SetPosition(2, Position((LandmarkIndex)23));

            lines[4].positionCount = 5;
            lines[4].SetPosition(0, Position((LandmarkIndex)24));
            lines[4].SetPosition(1, Position((LandmarkIndex)23));
            lines[4].SetPosition(2, Position((LandmarkIndex)11));
            lines[4].SetPosition(3, Position((LandmarkIndex)12));
            lines[4].SetPosition(4, Position((LandmarkIndex)24));

            lines[5].positionCount = 4;
            lines[5].SetPosition(0, Position((LandmarkIndex)12));
            lines[5].SetPosition(1, Position((LandmarkIndex)14));
            lines[5].SetPosition(2, Position((LandmarkIndex)16));
            lines[5].SetPosition(3, Position((LandmarkIndex)22));
            lines[6].positionCount = 4;
            lines[6].SetPosition(0, Position((LandmarkIndex)11));
            lines[6].SetPosition(1, Position((LandmarkIndex)13));
            lines[6].SetPosition(2, Position((LandmarkIndex)15));
            lines[6].SetPosition(3, Position((LandmarkIndex)21));

            lines[7].positionCount = 4;
            lines[7].SetPosition(0, Position((LandmarkIndex)16));
            lines[7].SetPosition(1, Position((LandmarkIndex)18));
            lines[7].SetPosition(2, Position((LandmarkIndex)20));
            lines[7].SetPosition(3, Position((LandmarkIndex)16));
            lines[8].positionCount = 4;
            lines[8].SetPosition(0, Position((LandmarkIndex)15));
            lines[8].SetPosition(1, Position((LandmarkIndex)17));
            lines[8].SetPosition(2, Position((LandmarkIndex)19));
            lines[8].SetPosition(3, Position((LandmarkIndex)15));

            lines[9].positionCount = 2;
            lines[9].SetPosition(0, Position((LandmarkIndex)10));
            lines[9].SetPosition(1, Position((LandmarkIndex)9));


            lines[10].positionCount = 5;
            lines[10].SetPosition(0, Position((LandmarkIndex)8));
            lines[10].SetPosition(1, Position((LandmarkIndex)5));
            lines[10].SetPosition(2, Position((LandmarkIndex)0));
            lines[10].SetPosition(3, Position((LandmarkIndex)2));
            lines[10].SetPosition(4, Position((LandmarkIndex)7));
        }

        public Vector3 Direction(LandmarkIndex from, LandmarkIndex to)
        {
            return (instances[(int)to].transform.position - instances[(int)from].transform.position).normalized;
        }

        public float Distance(LandmarkIndex from, LandmarkIndex to)
        {
            return (instances[(int)from].transform.position - instances[(int)to].transform.position).magnitude;
        }

        public Vector3 LocalPosition(LandmarkIndex Mark)
        {
            return instances[(int)Mark].transform.localPosition;
        }

        public Vector3 Position(LandmarkIndex Mark)
        {
            return instances[(int)Mark].transform.position;
        }
    }
}