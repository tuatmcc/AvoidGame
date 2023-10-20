using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignDemo
{
    public class Mover : MonoBehaviour
    {
        public float moveSpeed = 5f; // 移動速度

        void Update()
        {
            // WASDキーの入力を取得
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // 移動ベクトルを計算
            Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            // プレイヤーの位置を更新
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
