using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AvoidGame.Result.ModelController
{
    public class DirectionController : MonoBehaviour
    {
        [SerializeField] private Transform head;

        private async void Start()
        {
            await UniTask.WaitForSeconds(1);
            if (Camera.main != null) head.transform.LookAt(Camera.main.transform.position);
        }
    }
}