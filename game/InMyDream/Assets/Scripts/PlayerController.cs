using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // 이동 속도
    public float jumpForce = 5f;  // 점프 힘
    public float health = 1000f;  // 플레이어 체력
    public Transform groundCheck;  // 바닥 체크 지점
    public LayerMask groundLayer;  // 바닥으로 인식할 레이어
    private bool isGrounded;  // 바닥에 있는지 여부
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        // 좌우 및 상하 이동
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputZ = Input.GetAxis("Vertical");
        Vector3 moveVelocity = new Vector3(moveInputX * moveSpeed, rb.velocity.y, moveInputZ * moveSpeed);
        rb.velocity = moveVelocity;
    }

    void Jump()
    {
        // 바닥에 있을 때만 점프 가능
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어가 사망했습니다!");
        // 사망 처리 (예: 게임 오버 화면 띄우기)
    }
}

