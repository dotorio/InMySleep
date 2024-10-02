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
        Debug.Log("asd");

        if (rb != null)
        {
            // Rigidbody가 있다면 위쪽 방향으로 힘을 가함
            Debug.Log("dfgg");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}

// public class JumpPad : MonoBehaviour
// {
//     public float jumpForce = 30f; // 점프 힘을 설정하는 변수

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player")) // 플레이어와 충돌할 때만 실행
//         {
//             Rigidbody playerRb = other.GetComponent<Rigidbody>();
//             if (playerRb != null)
//             {
//                 playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z); // 위쪽으로 점프
//             }
//         }
//     }
// }

