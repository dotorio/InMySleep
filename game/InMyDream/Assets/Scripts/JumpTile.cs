using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTile : MonoBehaviour
{
    public float jumpForce = 10f; // 점프할 때 적용할 힘

    // 트리거 충돌이 발생했을 때 호출되는 메서드
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체에 Rigidbody가 있는지 확인
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Rigidbody가 있다면 위쪽 방향으로 힘을 가함
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
