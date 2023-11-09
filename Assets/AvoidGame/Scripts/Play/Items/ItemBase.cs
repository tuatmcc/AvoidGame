using AvoidGame.Audio;
using System;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Items
{
    /// <summary>
    /// アイテム
    /// </summary>
    public abstract class ItemBase : MonoBehaviour
    {
        public virtual bool DestroyOnItemCollectorHit => true;

        public event Action<Collider> OnItemCollectorHit;

        [Inject] private PlaySceneManager _playSceneManager;

        private void OnTriggerEnter(Collider other)
        {
            if (_playSceneManager.State != PlaySceneState.Playing) return;
            if (!other.TryGetComponent(out IItemCollectable _)) return;

            OnItemCollectorHit?.Invoke(other);

            if (DestroyOnItemCollectorHit)
            {
                Destroy(gameObject);
            }
        }
    }
}