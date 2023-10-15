using UnityEngine;

namespace Tracking
{
    public class BoneComposition
    {
        public Transform Parent { get; set; }
        public Quaternion InitialRotation { get; set; }

        public BoneComposition(Transform parent)
        {
            Parent = parent;
            InitialRotation = parent.rotation;
        }

        public void Rotate(Quaternion targetRotation, float speed)
        {
            Parent.rotation = Quaternion.Lerp(Parent.rotation, targetRotation, Time.deltaTime * speed);
        }
    }
}