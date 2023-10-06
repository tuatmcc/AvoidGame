using UnityEngine;

namespace Tracking
{
    public struct AccumulatedBuffer
    {
        public Vector3 value;
        public int accumulatedValuesCount;

        public AccumulatedBuffer(Vector3 v, int ac)
        {
            value = v;
            accumulatedValuesCount = ac;
        }
    }
}