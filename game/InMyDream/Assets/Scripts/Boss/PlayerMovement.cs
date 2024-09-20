using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 캐릭터 이동 속도
    private Rigidbody rb; // Rigidbody 컴포넌트

    private Vector3 moveDirection;

    void Start()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 물리적으로 회전하지 않게 설정
    }

    void Update()
    {
        // 입력 처리
        float moveX = Input.GetAxis("Horizontal"); // A, D 키 입력
        float moveZ = Input.GetAxis("Vertical"); // W, S 키 입력

        // 이동 방향 설정
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
    }

    void FixedUpdate()
    {
        // 물리적으로 이동
        if (moveDirection.magnitude >= 0.1f)
        {
            // 이동할 위치 계산
            Vector3 move = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }
}
