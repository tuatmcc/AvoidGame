using UnityEngine;

namespace Tracking
{
    /// <sumemary>
    /// Cache various values which will be reused during the runtime.
    /// </summary>
    public class CalibrationData
    {
        public Transform parent, child, tparent, tchild;
        public Vector3 initialDir;
        public Quaternion initialRotation;

        public Quaternion targetRotation;

        public void Tick(Quaternion newTarget, float speed)
        {
            parent.rotation = newTarget;
            parent.rotation = Quaternion.Lerp(parent.rotation, targetRotation, Time.deltaTime * speed);
        }

        public Vector3 CurrentDirection => (tchild.position - tparent.position).normalized;

        public CalibrationData(Transform fparent, Transform fchild, Transform tparent, Transform tchild)
        {
            initialDir = (tchild.position - tparent.position).normalized;
            initialRotation = fparent.rotation;
            this.parent = fparent;
            this.child = fchild;
            this.tparent = tparent;
            this.tchild = tchild;
        }
    }
}