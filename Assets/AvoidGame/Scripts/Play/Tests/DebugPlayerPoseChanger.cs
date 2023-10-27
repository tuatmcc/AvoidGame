using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace AvoidGame.Play.Test
{
    public class DebugPlayerPoseChanger : MonoBehaviour
    {
        [SerializeField] RigBuilder builder;
        private Animator _animator;

        private void Awake()
        {
            builder.enabled = false;
        }

        void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetInteger("Pose", 0);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _animator.SetInteger("Pose", 0);
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _animator.SetInteger("Pose", 1);
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _animator.SetInteger("Pose", 2);
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _animator.SetInteger("Pose", 3);
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _animator.SetInteger("Pose", 4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _animator.SetInteger("Pose", 5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                _animator.SetInteger("Pose", 6);
            }
        }
    }
}
